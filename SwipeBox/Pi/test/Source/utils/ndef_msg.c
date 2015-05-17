#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdarg.h>
#include "ndef_msg.h"

//-----------------------------------------------------------------------------
void MessageToOctets(uint8_t maxRecNr, uint8_t **octets, NDEF_Message *msg, uint32_t* mSize)
{
    uint32_t msgSize = 0;
    uint8_t k = 0;
    uint8_t i = 0;

    for(k=MSG_SIZE_MAX_OCT-1;k>=0;k--)
    {
        if(*(msg->ndefMessageLength+k) == 0)
            continue;
        else
            break;
    }

    for(i=k;i>0;i--)
    {
        msgSize = msgSize | *(msg->ndefMessageLength+i);
        msgSize = msgSize << OCTET;
    }
    msgSize = msgSize | *(msg->ndefMessageLength);

    *octets = malloc(msgSize+MSG_HEADER_MIN_SIZE+k);
    if(*octets != NULL)
    {
        uint8_t m = 0;
        uint32_t n = 0;
        uint32_t j = 0;
        uint32_t dataSize = 0;

        *(*octets+j++) = msg->ndefMessageType;
        for(k=MSG_SIZE_MAX_OCT-1;k>=0;k--)
        {
            if(*(msg->ndefMessageLength+k) == 0)
                continue;
            else
                break;
        }

        for(i=k;i>0;i--)
            *(*octets+j++) = *(msg->ndefMessageLength+i);

        *(*octets+j++) = *(msg->ndefMessageLength+i);

        for(i=0;i<maxRecNr;i++)
        {
            *(*octets+j++) = (msg->ndefMessageValue+i)->flags;
            *(*octets+j++) = (msg->ndefMessageValue+i)->typeLength;

            if(((msg->ndefMessageValue+i)->flags & SR_FLAG) == SR_FLAG)
            {
                *(*octets+j) = (msg->ndefMessageValue+i)->payloadLength[0];
                dataSize = *(*octets+j++);
            }
            else
            {
                *(*octets+j++) = (msg->ndefMessageValue+i)->payloadLength[0];
                *(*octets+j++) = (msg->ndefMessageValue+i)->payloadLength[1];
                *(*octets+j++) = (msg->ndefMessageValue+i)->payloadLength[2];
                *(*octets+j++) = (msg->ndefMessageValue+i)->payloadLength[3];
                dataSize = *(*octets+j-4) << OCTET;
                dataSize = (dataSize | *(*octets+j-3)) << OCTET;
                dataSize = (dataSize | *(*octets+j-2)) << OCTET;
                dataSize = dataSize | *(*octets+j-1);
            }

            if(((msg->ndefMessageValue+i)->flags & IL_FLAG) == IL_FLAG)
                *(*octets+j++) = (msg->ndefMessageValue+i)->idLength;

            *(*octets+j++) = (msg->ndefMessageValue+i)->type[0];

            for(n=0;n<(msg->ndefMessageValue+i)->idLength;n++)
                *(*octets+j++) = (msg->ndefMessageValue+i)->id[n];

            *(*octets+j++) = (msg->ndefMessageValue+i)->payload.typeOrParamLength;

            if((msg->ndefMessageValue+i)->payload.param == NULL)
                m = 0;
            else
            {
                for(m=0;m<(msg->ndefMessageValue+i)->payload.typeOrParamLength;m++)
                    *(*octets+j++) = (msg->ndefMessageValue+i)->payload.param[m];
            }
            
            for(n=0;n<dataSize-m;n++) {
                *(*octets+j++) = (msg->ndefMessageValue+i)->payload.data[n];
            }
            j--;
        }

        *(*octets+j++) = msg->terminatorTLV;
        *(*octets+j++) = '\0';
        *mSize = msgSize;
    }
}

//-----------------------------------------------------------------------------
// NAME: FirstRecord
//
// PARAMETERS: uint8_t *flags   Pointer to record flag
//
// OUTPUT: Status code 
// 
// PURPOSE: sets the first record flag
//-----------------------------------------------------------------------------
void FirstRecord(uint8_t *flags)
{
    *flags = *flags | MB_FLAG;
}

//-----------------------------------------------------------------------------
// NAME: LastRecord
//
// PARAMETERS: uint8_t *flags   Pointer to record flag
//
// OUTPUT: Status code
//
// PURPOSE: sets the last record flag
//-----------------------------------------------------------------------------
void LastRecord(uint8_t *flags)
{
    *flags = *flags | ME_FLAG;
}

//-----------------------------------------------------------------------------
// NAME: CreateMessage
//
// PARAMETERS: uint8_t maxRecNr    number of records in the message
//             NDEF_Message *msg   Pointer to store the NDEF message
//             NDEF_Record **rec   Pointer to (all) NDEF records
//
// OUTPUT: Status code
//
// PURPOSE: adds the message header to given record pointer
//-----------------------------------------------------------------------------
void CreateMessage(uint8_t maxRecNr, NDEF_Message *msg, NDEF_Record **rec)
{
    uint8_t i = 0;
    uint32_t msgSize = 0;
    uint8_t idSize = 0;

    msg->ndefMessageType = NDEF_MSG_OCTET;
    msg->ndefMessageLength = 0;

    msg->ndefMessageLength = calloc(MSG_SIZE_MAX_OCT,OCTET_SIZE);
    if(msg->ndefMessageLength != NULL)
    {
        for(i=0;i<maxRecNr;i++)
        {
            msgSize = 0;
            idSize = 0;
            if((*rec+i)->flags & IL_FLAG == IL_FLAG)
                idSize = OCTET_SIZE + (*rec+i)->idLength;
            if(((*rec+i)->flags & SR_FLAG) == SR_FLAG)
            {
                msgSize = (*rec+i)->payloadLength[0];
                msgSize = msgSize + (*rec+i)->typeLength + SR_HEADER_SIZE + idSize;
            }
            else
            {
                msgSize = (*rec+i)->payloadLength[0] << OCTET;
                msgSize = (msgSize | (*rec+i)->payloadLength[1]) << OCTET;
                msgSize = (msgSize | (*rec+i)->payloadLength[2]) << OCTET;
                msgSize = msgSize | (*rec+i)->payloadLength[3];

                msgSize = msgSize + (*rec+i)->typeLength + NR_HEADER_SIZE + idSize;
            }

            AdditionForBigValues(&(msg->ndefMessageLength),msgSize);
        }
    }
    
    msg->ndefMessageValue = *rec;
    msg->terminatorTLV = NDEF_MSG_END_OCTET;
}

//-----------------------------------------------------------------------------
// NAME: AdditionForBigValues
//
// PARAMETERS: uint8_t **retValue   Pointer for big numbers which are split into uint8_t parts
//             uint32_t addValue    big number to add to another number
//
// OUTPUT: Status code
//
// PURPOSE: addition of big numbers as octets
//-----------------------------------------------------------------------------
void AdditionForBigValues(uint8_t **retValue, uint32_t addValue)
{
    uint8_t i = 0;
    uint8_t carry = 0;
    uint8_t value = 0;

    for(i=0;i<MSG_SIZE_MAX_OCT;i++)
    {
        if(i == MSG_SIZE_MAX_OCT - 1)
        {
            *(*retValue+i) = *(*retValue+i) + carry;
        }
        else
        {
            value = *(*retValue+i);
            *(*retValue+i) = *(*retValue+i) + ((addValue >> OCTET*i) & MAX_OCTET_SIZE) + carry;
            if(*(*retValue+i) < value || *(*retValue+i) < ((addValue >> OCTET*i) & MAX_OCTET_SIZE))
                carry = OCTET_SIZE;
            else
                carry = 0;
        }
    }
}

//-----------------------------------------------------------------------------
// NAME: MergeAllRecords
//
// PARAMETERS: uint8_t maxRecNr       number of all records to merge
//             NDEF_Record **allRec   Pointer for the merged records
//             ...                    maxRecNr different records
//
// OUTPUT: Status code
//
// PURPOSE: merges all records to one pointer
//-----------------------------------------------------------------------------
void MergeAllRecords(uint8_t maxRecNr, NDEF_Record **allRec, ...)
{
    va_list recordList;
    va_start(recordList, *allRec);
    NDEF_Record *record;
    uint8_t i = 0;

    for(i=0;i<maxRecNr;i++)
    {
        record = va_arg(recordList, NDEF_Record*);
        if(i == 0)
            FirstRecord(&record->flags);
        if(i == maxRecNr-1)
            LastRecord(&record->flags);
        *(*allRec+i) = *record;
    }
}

//-----------------------------------------------------------------------------
// NAME: CreateRecordHeader
//
// PARAMETERS: NDEF_Record **recordHeader   record to store payload and record header
//             NDEF_Payload payloadData     payload for a record
//             uint32_t dataSize            size of the payload
//             uint8_t id[]                 id of the record
//             char type[]                  record type (text, uri,...)
//
// OUTPUT: Status code
//
// PURPOSE: adds the record header to the given record payload
//-----------------------------------------------------------------------------
void CreateRecordHeader(NDEF_Record **recordHeader, NDEF_Payload payloadData, uint32_t dataSize, uint8_t id[], char type[])
{
    uint8_t paramSize = 0;
    uint32_t paySize = 0;

    if(payloadData.data == NULL)
    {
        (*recordHeader)->flags = SR_FLAG | TNF_NFC_RTD_TYPE;
        (*recordHeader)->payloadLength[0] = dataSize;
    }
    else
    {
        if(payloadData.param == NULL)
            paramSize = URI_ID_CODE_SIZE;
        else
        paramSize = payloadData.typeOrParamLength + PARAM_SIZE_BYTE;

        paySize = paramSize + dataSize;

        if(paySize < dataSize || paySize < payloadData.typeOrParamLength)
            return;
        else
        {
            if(paySize <= SR_MAX_SIZE)
            {
                (*recordHeader)->flags = SR_FLAG | TNF_NFC_RTD_TYPE;
                (*recordHeader)->payloadLength[0] = paySize;
            }
            else
            {
                (*recordHeader)->flags = TNF_NFC_RTD_TYPE;
                (*recordHeader)->payloadLength[3] = paySize;
                (*recordHeader)->payloadLength[2] = paySize >> OCTET;
                (*recordHeader)->payloadLength[1] = paySize >> 2*OCTET;
                (*recordHeader)->payloadLength[0] = paySize >> 3*OCTET;
            }
        }
    }

    if(id != NULL)
    {
        (*recordHeader)->id = id;
        (*recordHeader)->idLength = strlen(id);
        (*recordHeader)->flags = (*recordHeader)->flags | IL_FLAG;
    }
    else
        (*recordHeader)->idLength = 0x00;

    (*recordHeader)->typeLength = strlen(type);

    (*recordHeader)->type = malloc((*recordHeader)->typeLength);
    if((*recordHeader)->type != NULL)
        (*recordHeader)->type = type;

    (*recordHeader)->payload = payloadData;
}

//-----------------------------------------------------------------------------
// NAME: CreateTextRecord
//
// PARAMETERS: NDEF_Record *rec     NDEF record to store payload and record header
//             uint8_t input[]      input for the payload
//             uint8_t type         type id for uri payload - UNUSED with parameter NO_IDCODE
//             uint8_t language[]   language id for text payload
//             uint8_t id[]         id of the record
//
// OUTPUT: Status code
//
// PURPOSE: transforms a given text in a ndef text record
//-----------------------------------------------------------------------------
void CreateTextRecord(NDEF_Record *rec, uint8_t input[], uint8_t type, uint8_t language[], uint8_t id[])
{
    NDEF_Payload msgPayload;
    if(strlen(input) == 0)
    { 
        msgPayload.typeOrParamLength = strlen(input);
        msgPayload.param = NULL;
        msgPayload.data = NULL;
    }
    else
    {
        msgPayload.typeOrParamLength = strlen(language);

        msgPayload.param = malloc(msgPayload.typeOrParamLength);
        if(msgPayload.param != NULL)
            msgPayload.param = language;
        else
            return;

        msgPayload.data = malloc(strlen(input));
        if(msgPayload.data != NULL)
            msgPayload.data = input;
        else
            return;
    }

    rec->payload = msgPayload;
    CreateRecordHeader(&rec, msgPayload, strlen(input), id, NDEF_TEXT);

//    if(msgPayload.param)
//        free(msgPayload.param);
//    
//    if(msgPayload.data)
//        free(msgPayload.data);
}

//-----------------------------------------------------------------------------
// NAME: CreateUriRecord
//
// PARAMETERS: NDEF_Record *rec   NDEF record to store payload and record header
//             uint8_t input[]    input for the payload
//             uint8_t type       type id for uri payload
//             uint8_t param[]    parameter id for text payload - UNUSED with parameter NO_PARAM
//             uint8_t id[]       id of the record
//
// OUTPUT: Status code
//
// PURPOSE: transforms a given uri in a ndef uri record
//-----------------------------------------------------------------------------
void CreateUriRecord(NDEF_Record *rec, uint8_t input[], uint8_t type, uint8_t param[], uint8_t id[])
{
    NDEF_Payload msgPayload;

    if(strlen(input) == 0)
    {
        msgPayload.typeOrParamLength = strlen(input);
        msgPayload.param = NULL;
        msgPayload.data = NULL;
    }
    else
    {
        msgPayload.typeOrParamLength = type;

        msgPayload.param = NULL;

        msgPayload.data = malloc(strlen(input));
        if(msgPayload.data != NULL)
            msgPayload.data = input;
        else
            return;
    }

    rec->payload = msgPayload;
    CreateRecordHeader(&rec, msgPayload, strlen(input), id, NDEF_URI);

    //free(msgPayload.data);
}
