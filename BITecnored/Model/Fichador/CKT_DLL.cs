//INSTANT C# NOTE: Formerly VB project-level imports:
//using Microsoft.VisualBasic.Compatibility.VB6;
//using System;
//using System.Collections;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.Windows.Forms;

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BITecnored.Model.Fichador
{
	public static class CKT_DLL
	{
		// Consts

		public const short CKT_ERROR_INVPARAM = -1;
		public const short CKT_ERROR_NETDAEMONREADY = -1;
		public const short CKT_ERROR_CHECKSUMERR = -2;
		public const short CKT_ERROR_MEMORYFULL = -1;
		public const short CKT_ERROR_INVFILENAME = -3;
		public const short CKT_ERROR_FILECANNOTOPEN = -4;
		public const short CKT_ERROR_FILECONTENTBAD = -5;
		public const short CKT_ERROR_FILECANNOTCREATED = -2;
		public const short CKT_ERROR_NOTHISPERSON = -1;

		public const short CKT_RESULT_OK = 1;
		public const short CKT_RESULT_ADDOK = 1;
		public const short CKT_RESULT_HASMORECONTENT = 2;


		//Public Const PERSONINFOSIZE As Short = 44
		public static short CLOCKINGRECORDSIZE;

        // Types

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct NETINFO
		{
			[MarshalAs(UnmanagedType.I4)]
			public int ID;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
			public byte[] IP;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
			public byte[] Mask;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
			public byte[] Gateway;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
			public byte[] ServerIP;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=6)]
			public byte[] MAC;
		}

		public struct DATETIMEINFO
		{
			public int ID;
			public short Year_Renamed;
			public byte Month_Renamed;
			public byte Day_Renamed;
			public byte Hour_Renamed;
			public byte Minute_Renamed;
			public byte Second_Renamed;
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct PERSONINFO
		{
			[MarshalAs(UnmanagedType.I4)]
			public int PersonID;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
			public byte[] Password;
			[MarshalAs(UnmanagedType.I4)]
			public int CardNo;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=12)]
			public byte[] Name;
			[MarshalAs(UnmanagedType.I4)]
			public int Dept; //部门
			[MarshalAs(UnmanagedType.I4)]
			public int Group; //部门
			[MarshalAs(UnmanagedType.I4)]
			public int KQOption; //考勤模式
			[MarshalAs(UnmanagedType.I4)]
			public int FPMark;
			[MarshalAs(UnmanagedType.I4)]
			public int Other; //特殊信息 =0 普通人员, =1 管理员
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct CLOCKINGRECORD
		{
			[MarshalAs(UnmanagedType.I4)]
			public int ID;
			[MarshalAs(UnmanagedType.I4)]
			public int PersonID;
			[MarshalAs(UnmanagedType.I4)]
			public int Stat;
			[MarshalAs(UnmanagedType.I4)]
			public int BackupCode;
			[MarshalAs(UnmanagedType.I4)]
			public int WorkTyte;
			//<VBFixedString(20), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=20)> _
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=20)]
			public byte[] Time;
		}

		public struct DEVICEINFO
		{
			public int ID;
			public int MajorVersion;
			public int MinorVersion;
			public int AdminPassword;
			public int DoorLockDelay;
			public int SpeakerVolume;
			public int Parameter;
			public int DefaultAuth;
			public int Capacity;
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct RINGTIME
		{
			[MarshalAs(UnmanagedType.I4)]
			public int hour;
			[MarshalAs(UnmanagedType.I4)]
			public int minute;
			[MarshalAs(UnmanagedType.I4)]
			public int week;
		}

		public struct TIMESECT
		{
			public byte bHour;
			public byte bMinute;
			public byte eHour;
			public byte eMinute;
		}


		public struct CKT_MessageInfo
		{
			[MarshalAs(UnmanagedType.I4)]
			public int PersonID;
			[MarshalAs(UnmanagedType.I4)]
			public int sYear;
			[MarshalAs(UnmanagedType.I4)]
			public int sMon;
			[MarshalAs(UnmanagedType.I4)]
			public int sDay;
			[MarshalAs(UnmanagedType.I4)]
			public int eYear;
			[MarshalAs(UnmanagedType.I4)]
			public int eMon;
			[MarshalAs(UnmanagedType.I4)]
			public int eDay;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=48)]
			public byte[] msg;
		}

		public struct CKT_MessageHead
		{
			[MarshalAs(UnmanagedType.I4)]
			public int PersonID;
			[MarshalAs(UnmanagedType.I4)]
			public int sYear;
			[MarshalAs(UnmanagedType.I4)]
			public int sMon;
			[MarshalAs(UnmanagedType.I4)]
			public int sDay;
			[MarshalAs(UnmanagedType.I4)]
			public int eYear;
			[MarshalAs(UnmanagedType.I4)]
			public int eMon;
			[MarshalAs(UnmanagedType.I4)]
			public int eDay;
		}

		// Routines

		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_FreeMemory", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_FreeMemory(int memory);

        
        [System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_RegisterSno", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_RegisterSno(int Sno, int ComPort);
		//Public Declare Function CKT_RegisterNet Lib "tc400.dll" (ByVal Sno As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal Addr As String) As Integer
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_RegisterNet", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_RegisterNet(int Sno, string Addr);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_UnregisterSnoNet", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern void CKT_UnregisterSnoNet(int Sno);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_NetDaemon", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_NetDaemon();
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ComDaemon", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ComDaemon();
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_Disconnect", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern void CKT_Disconnect();
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ReportConnections", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ReportConnections(ref int ppSno);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetDeviceNetInfo", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetDeviceNetInfo(int Sno, ref NETINFO pNetInfo);
		//Public Declare Function CKT_SetDeviceIPAddr Lib "tc400.dll" (ByVal Sno As Integer, ByRef IP As Byte) As Integer
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceIPAddr", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceIPAddr(int Sno, byte[] IP);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceMask", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceMask(int Sno, ref byte Mask);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceGateway", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceGateway(int Sno, ref byte Gate);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceServerIPAddr", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceServerIPAddr(int Sno, ref byte Svr);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceMAC", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceMAC(int Sno, ref byte MAC);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetDeviceClock", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetDeviceClock(int Sno, ref DATETIMEINFO pDateTimeInfo);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceDate", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceDate(int Sno, short Year_Renamed, byte Month_Renamed, byte Day_Renamed);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceTime", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceTime(int Sno, byte Hour_Renamed, byte Minute_Renamed, byte Second_Renamed);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetFPTemplate", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetFPTemplate(int Sno, int PersonID, int FPID, ref int pFPData, ref int FPDataLen);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_PutFPTemplate", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_PutFPTemplate(int Sno, int PersonID, int FPID, byte[] pFPData, int FPDataLen);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetFPTemplateSaveFile", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetFPTemplateSaveFile(int Sno, int PersonID, int FPID, string FPDataFilename);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_PutFPTemplateLoadFile", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_PutFPTemplateLoadFile(int Sno, int PersonID, int FPID, string FPDataFilename);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetFPRawData", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetFPRawData(int Sno, int PersonID, int FPID, ref byte FPRawData);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_PutFPRawData", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_PutFPRawData(int Sno, int PersonID, int FPID, ref byte FPRawData);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetFPRawDataSaveFile", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetFPRawDataSaveFile(int Sno, int PersonID, int FPID, string FPDataFilename);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_PutFPRawDataLoadFile", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_PutFPRawDataLoadFile(int Sno, int PersonID, int FPID, string FPDataFilename);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ListPersonInfo", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ListPersonInfo(int Sno, ref int pRecordCount, ref int ppPersons);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ModifyPersonInfo", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ModifyPersonInfo(int Sno, ref PERSONINFO person);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_DeletePersonInfo", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_DeletePersonInfo(int Sno, int PersonID, int backupID);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_EraseAllPerson", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_EraseAllPerson(int Sno);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ListPersonInfoEx", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ListPersonInfoEx(int Sno, ref int ppLongRun);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ListPersonProgress", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ListPersonProgress(int pLongRun, ref int pRecCount, ref int pRetCount, ref uint ppPersons);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetCounts", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetCounts(int Sno, ref int pPersonCount, ref int pFPCount, ref int pClockingsCount);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ClearClockingRecord", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ClearClockingRecord(int Sno, int type, int count);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetClockingRecordEx", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetClockingRecordEx(int Sno, ref int ppLongRun);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetClockingNewRecordEx", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetClockingNewRecordEx(int Sno, ref int ppLongRun);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetClockingRecordProgress", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetClockingRecordProgress(int pLongRun, ref int pRecCount, ref int pRetCount, ref int ppPersons);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ResetDevice", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ResetDevice(int Sno);

		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetDeviceInfo", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetDeviceInfo(int Sno, ref DEVICEINFO devinfo);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDefaultAuth", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDefaultAuth(int Sno, int Auth);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDoor", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDoor(int Sno, int Second_Renamed);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetSpeakerVolume", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetSpeakerVolume(int Sno, int Volume);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetDeviceAdminPassword", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetDeviceAdminPassword(int Sno, [MarshalAs(UnmanagedType.LPStr)] string Password);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetRealtimeMode", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetRealtimeMode(int Sno, int RealMode);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetFixWGHead", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetFixWGHead(int Sno, int WGHead);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetWG", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetWG(int Sno, int WGMode);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetRingAllow", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetRingAllow(int Sno, int type);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetRepeatKQ", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetRepeatKQ(int Sno, int time);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetAutoUpdate", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetAutoUpdate(int Sno, int AutoUpdate);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ForceOpenLock", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ForceOpenLock(int Sno);


		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_ReadRealtimeClocking", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_ReadRealtimeClocking(ref int ppClockings);

		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetTimeSection", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetTimeSection(int Sno, int ord, out TIMESECT[] ts);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetTimeSection", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetTimeSection(int Sno, int ord, [In()] TIMESECT[] ts);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetGroup", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetGroup(int Sno, int ord, out int[] grp);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetGroup", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetGroup(int Sno, int ord, int[] grp);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetHitRingInfo", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetHitRingInfo(int Sno, out RINGTIME[] array);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_SetHitRingInfo", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_SetHitRingInfo(int Sno, int ord, ref RINGTIME ring);

		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetMessageByIndex", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetMessageByIndex(int Sno, int idx, ref CKT_MessageInfo msg);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_AddMessage", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_AddMessage(int Sno, ref CKT_MessageInfo msg);
		//Public Declare Function CKT_GetAllMessageHead Lib "tc400.dll" (ByVal Sno As Integer, <[In](), Out()> ByVal mh As CKT_MessageHead()) As Integer
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_GetAllMessageHead", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_GetAllMessageHead(int Sno, out CKT_MessageHead[] mh);
		[System.Runtime.InteropServices.DllImport("Model\\Fichador\\tc400.dll", EntryPoint="CKT_DelMessageByIndex", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
		public static extern int CKT_DelMessageByIndex(int Sno, int idx);


	}
}