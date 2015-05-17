#ifndef NDEF_MSG_H_INCLUDED
#define NDEF_MSG_H_INCLUDED

#include "ph_Status.h"

// Record Definition Type
#define NDEF_TEXT           "T"
#define NDEF_URI            "U"
#define NDEF_SMART_POSTER   "SP"

// Language
#define LANG_EN             "en"
#define LANG_DE             "de"
#define LANG_FR             "de"
#define LANG_JA             "ja"
#define LANG_EN_US          "en-US"
#define LANG_EN_UK          "en-UK"

// URI Identifier Code
#define URI_0               0x00
#define URI_HTTP_WWW        0x01
#define URI_HTTPS_WWW       0x02
#define URI_HTTP            0x03
#define URI_HTTPS           0x04
#define URI_TEL             0x05
#define URI_MAILTO          0x06
#define URI_FTP_ANONYMOUS   0x07
#define URI_FTP_FTP         0x08
#define URI_FTPS            0x09
#define URI_SFTP            0x0A
#define URI_SMB             0x0B
#define URI_NFS             0x0C
#define URI_FTP             0x0D
#define URI_DAV             0x0E
#define URI_NEWS            0x0F
#define URI_TELNET          0x10
#define URI_IMAP            0x11
#define URI_RTSP            0x12
#define URI_URN             0x13
#define URI_POP             0x14
#define URI_SIP             0x15
#define URI_SIPS            0x16
#define URI_TFTP            0x17
#define URI_BTSPP           0x18
#define URI_ BTL2CAP        0x19
#define URI_BTGOEP          0x1A
#define URI_TCPOBEX         0x1B
#define URI_IRDAOBEX        0x1C
#define URI_FILE            0x1D
#define URI_URN_EPC_ID      0x1E
#define URI_URN_EPC_TAG     0x1F
#define URI_URN_EPC_PAT     0x20
#define URI_URN_EPC_RAW     0x21
#define URI_URN_EPC         0x22
#define URI_URN_NFC         0x23

// size of URI Identifier Code
#define URI_ID_CODE_SIZE    0x01

// maximal size of Short Records
#define SR_MAX_SIZE         0xFF
#define NR_MAX_SIZE         0xFFFFFFFF

// flags
#define MB_FLAG             0x80
#define ME_FLAG             0x40
// this flag is set when the record is "short"
#define SR_FLAG             0x10
#define IL_FLAG             0x08

//
#define NDEF_MSG_OCTET      0x03
#define NDEF_MSG_END_OCTET  0xFE

//
#define PARAM_SIZE_BYTE     0x01
#define NO_IDCODE           0x00
#define NO_PARAM            NULL

#define MSG_SIZE_MAX_OCT    5
#define MAX_OCTET_SIZE      0xFF

#define SR_HEADER_SIZE      3
#define NR_HEADER_SIZE      6
#define MSG_HEADER_MIN_SIZE 3

#define OCTET               8
#define OCTET_SIZE          1

#define TNF_NFC_RTD_TYPE    0x01

typedef struct{
  uint8_t typeOrParamLength;
  uint8_t *param;
  uint8_t *data;
}NDEF_Payload;

typedef	struct
{
  uint8_t  flags;
  uint8_t  typeLength;
  uint8_t  payloadLength[4];
  uint8_t idLength;	// (existence depends on IL flag)
  uint8_t  *type;
  uint8_t *id;	// (existence depends on IL flag)
  NDEF_Payload payload;
}NDEF_Record;

typedef struct {
  uint8_t ndefMessageType;
  uint8_t *ndefMessageLength;
  NDEF_Record *ndefMessageValue;
  uint8_t terminatorTLV;
}NDEF_Message;

void MessageToOctets(uint8_t maxRecNr, uint8_t **octets, NDEF_Message *msg, uint32_t* mSize);
void FirstRecord(uint8_t *flags);
void LastRecord(uint8_t *flags);
void CreateMessage(uint8_t maxRecNr, NDEF_Message *msg, NDEF_Record **rec);
void AdditionForBigValues(uint8_t **retValue, uint32_t addValue);
void MergeAllRecords(uint8_t maxRecNr, NDEF_Record **allRec, ...);
void CreateRecordHeader(NDEF_Record **recordHeader, NDEF_Payload payloadData, uint32_t dataSize, uint8_t id[], char type[]);
void CreateTextRecord(NDEF_Record *rec, uint8_t input[], uint8_t type, uint8_t language[], uint8_t id[]);
void CreateUriRecord(NDEF_Record *rec, uint8_t input[], uint8_t type, uint8_t param[], uint8_t id[]);

#endif // NDEF_MSG_H_INCLUDED
