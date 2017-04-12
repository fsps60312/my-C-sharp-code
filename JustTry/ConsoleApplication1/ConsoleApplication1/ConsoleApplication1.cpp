#include<windows.h>
#include<stdio.h>
#include"WinIo.h"

#define KBC_CMD 0x64
#define KBC_DATA 0x60
void KBCWait4IBE()
{
	DWORD dwVal = 0;
	do
	{
		GetPortVal(KBC_CMD, &dwVal, 1);
	} while ((&dwVal) && (0x2) == 0);
}
void KEY_DOWN(int vk_in)
{
	int myscancode;
	myscancode = MapVirtualKey(byte(vk_in), 0);
	KBCWait4IBE();
	SetPortVal(KBC_CMD, 0xD2, 1);
	KBCWait4IBE();
	SetPortVal(KBC_DATA, 0xE2, 1);
	KBCWait4IBE();
	SetPortVal(KBC_CMD, 0xD2, 1);
	KBCWait4IBE();
	SetPortVal(KBC_DATA, myscancode, 1);
}
void KEY_UP(int vk_in)
{
	int myscancode;
	myscancode = MapVirtualKey(byte(vk_in), 0);
	KBCWait4IBE();
	SetPortVal(KBC_CMD, 0xD2, 1);
	KBCWait4IBE();
	SetPortVal(KBC_DATA, 0xE0, 1);
	KBCWait4IBE();
	SetPortVal(KBC_CMD, 0xD2, 1);
	KBCWait4IBE();
	SetPortVal(KBC_DATA, (myscancode | 0x80), 1);
}
void main()
{
	bool br, br1;
	//br=InitializeWinIo(); //in NT/XP no need
	//if (br==false)
	//{
	// MessageBox(NULL,"初始化winio失败,程序自动关闭,请您不用担心~","XD友情提示1",MB_OK);
	// ShutdownWinIo();
	// exit(0);
	//}
	br1 = InstallWinIoDriver("WinIo.sys");
	if (br1 == false)
	{
		MessageBox(NULL, "安装IO_device失败,程序自动关闭,请您不用担心~", "XD友情提示2", MB_OK);
		RemoveWinIoDriver();
		ShutdownWinIo();
		exit(0);
	}
	printf("安装成功1!!! press Enter to continue...");
	getchar();
	InitializeWinIo();
	printf("安装成功2!!! press Enter to continue...");
	getchar();
	for (int ii = 0; ii <= 20; ii++)
	{
		KEY_DOWN(65);//模拟按键
		Sleep(200);
		KEY_UP(65);//
	}

	printf("done!!!");
	getchar();
	RemoveWinIoDriver();
	ShutdownWinIo();
}