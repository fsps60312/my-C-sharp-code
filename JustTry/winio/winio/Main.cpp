#include   "windows.h " 
#include   "WinIo.h " 
#include   "winuser.h " 
#include   "iostream.h " 
#define   VK_A   0x41

#define   KBC_KEY_CMD   0x64         //键盘命令端口 
#define   KBC_KEY_DATA   0x60       //键盘数据端口


void   KBCWait4IBE()
{
	DWORD   dwRegVal = 0;
	do
	{
		GetPortVal(0x64, &dwRegVal, 1);
	} while (dwRegVal & 0x00000001);
}

void   MyKeyDownEx(long   vKeyCoad)       //模拟扩展键按下，参数vKeyCoad是扩展键的虚拟码 
{
	long   btScancode;
	btScancode = MapVirtualKey(vKeyCoad, 0);

	KBCWait4IBE();       //等待键盘缓冲区为空 
	SetPortVal(KBC_KEY_CMD, 0xD2, 1);       //发送键盘写入命令 
	KBCWait4IBE();
	SetPortVal(KBC_KEY_DATA, 0xE0, 1);   //写入扩展键标志信息 


	KBCWait4IBE();       //等待键盘缓冲区为空 
	SetPortVal(KBC_KEY_CMD, 0xD2, 1);         //发送键盘写入命令 
	KBCWait4IBE();
	SetPortVal(KBC_KEY_DATA, btScancode, 1);   //写入按键信息,按下键 
}


void   MyKeyUpEx(long   vKeyCoad)       //模拟扩展键弹起 
{
	long   btScancode;
	btScancode = MapVirtualKey(vKeyCoad, 0);

	KBCWait4IBE();       //等待键盘缓冲区为空 
	SetPortVal(KBC_KEY_CMD, 0xD2, 1);         //发送键盘写入命令 
	KBCWait4IBE();
	SetPortVal(KBC_KEY_DATA, 0xE0, 1);   //写入扩展键标志信息 


	KBCWait4IBE();     //等待键盘缓冲区为空 
	SetPortVal(KBC_KEY_CMD, 0xD2, 1);         //发送键盘写入命令 
	KBCWait4IBE();
	SetPortVal(KBC_KEY_DATA, (btScancode | 0x80), 1);     //写入按键信息，释放键 
}

void   MyKeyDown(long   vKeyCoad)
{
	long   byScancode = MapVirtualKey(vKeyCoad, 0);
	KBCWait4IBE();//等待键盘缓冲区为空   
	if (SetPortVal(0x64, 0xD2, 1) == false)cout < < "发送键盘写入命令失败! " < <endl;//发送键盘写入命令

	KBCWait4IBE();//等待键盘缓冲区为空   
	if (SetPortVal(0x60, (ULONG)byScancode, 1) == false)cout < < "按下键失败! " < <endl;//写入按键信息,按下键 
}


void   MyKeyUp(long   vKeyCoad)
{
	long   byScancode = MapVirtualKey(vKeyCoad, 0);
	KBCWait4IBE();//等待键盘缓冲区为空   
	if (SetPortVal(0x64, 0xD2, 1) == false)cout < < "发送键盘写入命令失败! " < <endl;//发送键盘写入命令

	KBCWait4IBE();//等待键盘缓冲区为空   
	if (SetPortVal(0x60, (ULONG)(byScancode | 0x80), 1) == false)cout < < "释放键失败! " < <endl;//写入按键信息，释放键 
}


void   main()
{
	if (InitializeWinIo() == false)cout < < "驱动程序加载失败! " < <endl;

	Sleep(3000);

	MyKeyDownEx(VK_LEFT);       //按下左方向键 
	Sleep(200);                         //延时200毫秒 
	MyKeyUpEx(VK_LEFT);           //释放左方向键   
	Sleep(500);
	MyKeyDown(VK_SPACE);       //按下空格键，注意要发送两次 
	MyKeyDown(VK_SPACE);
	Sleep(200);
	MyKeyUp(VK_SPACE);       //释放空格键

	ShutdownWinIo();
}