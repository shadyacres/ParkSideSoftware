/**********************************************************
*	Company: Park Side Software
*	Product: SwipeBox
*	Developer: James Pagan-Lodge
*	Version: 0.1.5.0 (Major.Minor.Revision.Bug)
*	Date: 14/02/2015
**********************************************************/

// Includes
#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <stdint.h>
#include <fcntl.h>
#include <string.h>
#include <ph_NxpBuild.h>		// Controls build behavior of components
#include <ph_Status.h>			// Status code definitions
#include <phpalI14443p3a.h>		// Generic ISO14443-3A Component of Reader Library Framework
#include <phpalI14443p4.h>		// Generic ISO14443-4 Component of Reader Library Framework
#include <phpalI14443p4a.h>		// Generic ISO14443-4A Component of Reader Library Framework
#include <phalMful.h>			// Generic MIFARE(R) Ultralight Application Component of Reader Library Framework
#include <phalMfc.h>
#include <phKeyStore.h>			// Generic KeyStore Component of Reader Library Framework. We need the key components for some function calls and it might be need when using crypto with Ultralight-C cards.
#include <phpalSli15693.h>
#include <phpalSli15693_Sw.h>
#include <phpalFelica.h>
#include <phpalI14443p3b.h>

// Defines
#define sak_ul                0x00
#define sak_ulc               0x00
#define sak_mini              0x09
#define sak_mfc_1k            0x08
#define sak_mfc_4k            0x18
#define sak_mfp_2k_sl1        0x08
#define sak_mfp_4k_sl1        0x18
#define sak_mfp_2k_sl2        0x10
#define sak_mfp_4k_sl2        0x11
#define sak_mfp_2k_sl3        0x20
#define sak_mfp_4k_sl3        0x20
#define sak_desfire           0x20
#define sak_jcop              0x28
#define sak_layer4            0x20

#define atqa_ul               0x4400
#define atqa_ulc              0x4400
#define atqa_mfc              0x0200
#define atqa_mfp_s            0x0400
#define atqa_mfp_s_2K         0x4400
#define atqa_mfp_x            0x4200
#define atqa_desfire          0x4403
#define atqa_jcop             0x0400
#define atqa_mini             0x0400
#define atqa_nPA              0x0800

#define mifare_ultralight     0x01
#define mifare_ultralight_c   0x02
#define mifare_classic        0x03
#define mifare_classic_1k     0x04
#define mifare_classic_4k     0x05
#define mifare_plus           0x06
#define mifare_plus_2k_sl1    0x07
#define mifare_plus_4k_sl1    0x08
#define mifare_plus_2k_sl2    0x09
#define mifare_plus_4k_sl2    0x0A
#define mifare_plus_2k_sl3    0x0B
#define mifare_plus_4k_sl3    0x0C
#define mifare_desfire        0x0D
#define jcop                  0x0F
#define mifare_mini           0x10
#define nPA                   0x11

// Forward declarations
uint32_t DetectMifare(void *halReader);
phStatus_t readerIC_Cmd_SoftReset(void *halReader);

void WriteToFile(char *pID);
void SendToWebService(uint8_t *pID[10]);


/*************************************************
*	MAIN
*************************************************/
int main(int argc, char **argv)
{
	printf("\n\nSTART\n");
	printf("-----------------------------------\n");
	
	phbalReg_R_Pi_spi_DataParams_t spi_balReader;
    void *balReader;

    phhalHw_Rc523_DataParams_t halReader;
    void *ptrHal;
    phStatus_t status;
    uint8_t blueboardType;
    uint8_t volatile card_or_tag_detected;

    uint8_t bHalBufferReader[0x40];
	
	// BEGIN: Hardware INITIALIZATION
	// Initialize the Reader BAL (Bus Abstraction Layer) component

    printf("SPI init\n");
    printf("--------------------------\n");
    printf("Enter: phbalReg_R_Pi_spi_Init\n");
	
	status = phbalReg_R_Pi_spi_Init(&spi_balReader, sizeof(phbalReg_R_Pi_spi_DataParams_t));

    if (PH_ERR_SUCCESS != status) {
        printf("Failed to initialize SPI\n");
        return 1;
    }
	else {
		printf("OK - phbalReg_R_Pi_spi_Init\n");
		printf("wId:        %0x\n",spi_balReader.wId);
		printf("spiFD:      %0x\n",spi_balReader.spiFD);
		printf("spiMode:    %0x\n",spi_balReader.spiMode);
		printf("spiBPW:     %0x\n",spi_balReader.spiBPW);
		printf("spiDelay:   %0x\n",spi_balReader.spiDelay);
		printf("spiSpeed:   %0x\n",spi_balReader.spiSpeed);
		printf("   (MHz):   %0d\n",spi_balReader.spiSpeed/1000000);
	}
	printf("--------------------------\n");
	
	balReader = (void *)&spi_balReader;

    status = phbalReg_OpenPort((void*)balReader);

    if (PH_ERR_SUCCESS != status) {
        printf("Failed to open bal\n");
        return 2;
    }
	
	status = phhalHw_Rc523_Init(&halReader, sizeof(phhalHw_Rc523_DataParams_t), balReader, 0, bHalBufferReader, sizeof(bHalBufferReader), bHalBufferReader, sizeof(bHalBufferReader));

    ptrHal = &halReader;

    if (PH_ERR_SUCCESS != status) {
        printf("Failed to initialize the HAL\n");
        return 3;
    }

    /* Set the HAL configuration to SPI */
    status = phhalHw_SetConfig(ptrHal, PHHAL_HW_CONFIG_BAL_CONNECTION, PHHAL_HW_BAL_CONNECTION_SPI);

    if (PH_ERR_SUCCESS != status) {
        printf("Failed to set hal connection SPI\n");
        return 4;
    }
	
	printf("--------------------------\n");
    printf("field reset\n");
    printf("--------------------------\n");
	
	status = readerIC_Cmd_SoftReset(ptrHal);
	
	printf("/******Begin Polling******/\n");
	
	// Begin polling for cards
	while(1) {
		// Detect MiFare cards
		if(DetectMifare(ptrHal)) {
			// Reset the IC
			readerIC_Cmd_SoftReset(ptrHal);
		}
		else {
			printf("No card detected\n");
		}
		
		sleep(1);
	}
	
	phhalHw_FieldOff(ptrHal);
    return 0;
}

/*************************************************
*	Resetting the IC
*************************************************/
phStatus_t readerIC_Cmd_SoftReset(void *halReader)
{
    phStatus_t status = PH_ERR_INVALID_DATA_PARAMS;

    switch (PH_GET_COMPID(halReader)) {
		case PHHAL_HW_RC523_ID:
        status = phhalHw_Rc523_Cmd_SoftReset(halReader);
		break;
    }

    return status;
}

/******************************************************
*	Detects MiFare NFC cards
******************************************************/
uint32_t DetectMifare(void *halReader)
{
    phpalI14443p4_Sw_DataParams_t I14443p4;
    phpalMifare_Sw_DataParams_t palMifare;
    phpalI14443p3a_Sw_DataParams_t I14443p3a;

    uint8_t cryptoEnc[8];
    uint8_t cryptoRng[8];

    phalMful_Sw_DataParams_t alMful;

    uint8_t bUid[10];
    uint8_t bLength;
    uint8_t bMoreCardsAvailable;
    uint32_t sak_atqa = 0;
    uint8_t pAtqa[2];
    uint8_t bSak[1];
    phStatus_t status;
    uint16_t detected_card = 0xFFFF;

    /* Initialize the 14443-3A PAL (Protocol Abstraction Layer) component */
    PH_CHECK_SUCCESS_FCT(status, phpalI14443p3a_Sw_Init(&I14443p3a, sizeof(phpalI14443p3a_Sw_DataParams_t), halReader));

    /* Initialize the 14443-4 PAL component */
    PH_CHECK_SUCCESS_FCT(status, phpalI14443p4_Sw_Init(&I14443p4, sizeof(phpalI14443p4_Sw_DataParams_t), halReader));

    /* Initialize the Mifare PAL component */
    PH_CHECK_SUCCESS_FCT(status, phpalMifare_Sw_Init(&palMifare, sizeof(phpalMifare_Sw_DataParams_t), halReader, &I14443p4));

    /* Initialize Ultralight(-C) AL component */
    PH_CHECK_SUCCESS_FCT(status, phalMful_Sw_Init(&alMful, sizeof(phalMful_Sw_DataParams_t), &palMifare, NULL, NULL, NULL));

    /* Reset the RF field */
    PH_CHECK_SUCCESS_FCT(status, phhalHw_FieldReset(halReader));

    /* Apply the type A protocol settings and activate the RF field. */
    PH_CHECK_SUCCESS_FCT(status, phhalHw_ApplyProtocolSettings(halReader, PHHAL_HW_CARDTYPE_ISO14443A));

    /* Empty the pAtqa */
    memset(pAtqa, '\0', 2);
    status = phpalI14443p3a_RequestA(&I14443p3a, pAtqa);

    /* Reset the RF field */
    PH_CHECK_SUCCESS_FCT(status, phhalHw_FieldReset(halReader));

    /* Empty the bSak */
    memset(bSak, '\0', 1);

    /* Activate one card after another and check it's type. */
	bMoreCardsAvailable = 1;
	uint8_t cards = 0;

	while (bMoreCardsAvailable)	{
		cards++;
		/* Activate the communication layer part 3 of the ISO 14443A standard. */
		status = phpalI14443p3a_ActivateCard(&I14443p3a, NULL, 0x00, bUid, &bLength, bSak, &bMoreCardsAvailable);
		uint8_t pUidOut[10];
		sak_atqa = bSak[0] << 24 | pAtqa[0] << 8 | pAtqa[1];
		sak_atqa &= 0xFFFF0FFF;

		if (!status) {
			// Detect mini or classic
			switch (sak_atqa) {
				case sak_mfc_1k << 24 | atqa_mfc:
				printf("MIFARE Classic detected\n");
				detected_card &= mifare_classic;
				break;
				
				case sak_mfc_4k << 24 | atqa_mfc:
				printf("MIFARE Classic detected\n");
				detected_card &= mifare_classic;
				break;
				
				case sak_mfp_2k_sl1 << 24 | atqa_mfp_s:
				printf("MIFARE Classic detected\n");
				detected_card &= mifare_classic;
				break;
				
				case sak_mini << 24 | atqa_mini:
				printf("MIFARE Mini detected\n");
				detected_card &= mifare_mini;
				break;
				
				case sak_mfp_4k_sl1 << 24 | atqa_mfp_s:
				printf("MIFARE Classic detected\n");
				detected_card &= mifare_classic;
				break;
				
				case sak_mfp_2k_sl1 << 24 | atqa_mfp_x:
				printf("MIFARE Classic detected\n");
				detected_card &= mifare_classic;
				break;
				
				case sak_mfp_4k_sl1 << 24 | atqa_mfp_x:
				printf("MIFARE Classic detected\n");
				detected_card &= mifare_classic;
				break;
				
				default:
				break;
			}

			if (detected_card == 0xFFFF) {
				sak_atqa = bSak[0] << 24 | pAtqa[0] << 8 | pAtqa[1];
				switch (sak_atqa) {
					case sak_ul << 24 | atqa_ul:
					printf("MIFARE Ultralight detected\n");
					detected_card &= mifare_ultralight;
					break;
					
					case sak_mfp_2k_sl2 << 24 | atqa_mfp_s:
					printf("MIFARE Plus detected\n");
					detected_card &= mifare_plus;
					break;
					
					case sak_mfp_2k_sl3 << 24 | atqa_mfp_s_2K:
					printf("MIFARE Plus detected\n");
					detected_card &= mifare_plus;
					break;
					
					case sak_mfp_2k_sl3 << 24 | atqa_mfp_s:
					printf("MIFARE Plus detected\n");
					detected_card &= mifare_plus;
					break;
					
					case sak_mfp_4k_sl2 << 24 | atqa_mfp_s:
					printf("MIFARE Plus detected\n");
					detected_card &= mifare_plus;
					break;
					
					case sak_mfp_2k_sl2 << 24 | atqa_mfp_x:
					printf("MIFARE Plus detected\n");
					detected_card &= mifare_plus;
					break;
					
					case sak_mfp_2k_sl3 << 24 | atqa_mfp_x:
					printf("MIFARE Plus detected\n");
					detected_card &= mifare_plus;
					break;
					
					case sak_mfp_4k_sl2 << 24 | atqa_mfp_x:
					printf("MIFARE Plus detected\n");
					detected_card &= mifare_plus;
					break;
					
					case sak_desfire << 24 | atqa_desfire:
					printf("MIFARE DESFire detected\n");
					detected_card &= mifare_desfire;
					break;
					
					default:
					break;
				}
			}
		}
		else
			// No MIFARE card is in the field
			return false;

		// There is a MIFARE card in the field, but we cannot determine it
		if (!status && detected_card == 0xFFFF) {
			printf("MIFARE card detected\n");
			return true;
		}
		
		printf("UID: ");
		uint8_t i;
		
		for(i = 0; i < bLength; i++) {
			printf("%02X ", bUid[i]);
		}
		
		printf("\n");
		
		// Read data on the card
		uint8_t bNDEFBuffer[48];
		phalMful_Read(&alMful, 4, bNDEFBuffer);
		const int indexStart = 9;
		const int indexFinish = 15;
		uint8_t bCusID[10];
		
		// print out the NDEF record
		printf("NDEF record: ");
		int j;
		for(j = 0; j < 48; j++) {
			printf("%02X ", bNDEFBuffer[j]);
		}
		
		memcpy(bCusID, &bNDEFBuffer[indexStart], indexFinish - indexStart);
		
		printf("CusID: ");
		int p;
		for(p = 0; p < 10; p++) {
			printf("%02X ", bCusID[p]);
		}
		printf("%s\n", bCusID);
						
		printf("\n\n");
				
		status = phpalI14443p3a_HaltA(&I14443p3a);
		detected_card = 0xFFFF;
	}
	return detected_card;
}

void WriteToFile(char *pID)
{
	printf("Writing to file. . . . .\n");
	
	printf("%s\n", pID);
}

void SendToWebService(uint8_t *pID[10])
{
	
}
