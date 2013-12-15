// beanulator's code is licensed under the 4 clause BSD license:
//
// Copyright (c) 2013, beannaich
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by beannaich.
// 4. Neither the name of beanulator nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDERS BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using single = System.Single;
using uint32 = System.UInt32;

namespace Beanulator.Common.Processors.HITACHI
{
    public abstract class SH7750 : Processor
    {
        private single[] fpr = new single[32];
        private uint32[] gpr = new uint32[16];

        #region bus controller

        // Name										Abbreviation	R/W		Initial Value			P4 Address			Area 7 Address		Access Size
        // Bus control register 1					BCR1			R/W		H'0000 0000				H'FF80 0000			H'1F80 0000			32
        // Bus control register 2					BCR2			R/W		H'3FFC					H'FF80 0004			H'1F80 0004			16
        // Wait state control register 1			WCR1			R/W		H'7777 7777				H'FF80 0008			H'1F80 0008			32
        // Wait state control register 2			WCR2			R/W		H'FFFE EFFF				H'FF80 000C			H'1F80 000C			32
        // Wait state control register 3			WCR3			R/W		H'0777 7777				H'FF80 0010			H'1F80 0010			32
        // Memory control register					MCR				R/W		H'0000 0000				H'FF80 0014			H'1F80 0014			32
        // PCMCIA control register					PCR				R/W		H'0000					H'FF80 0018			H'1F80 0018			16
        // Refresh timer control/status register	RTCSR			R/W		H'0000					H'FF80 001C			H'1F80 001C			16
        // Refresh timer counter					RTCNT			R/W		H'0000					H'FF80 0020			H'1F80 0020			16
        // Refresh time constant counter			RTCOR			R/W		H'0000					H'FF80 0024			H'1F80 0024			16
        // Refresh count register					RFCR			R/W		H'0000					H'FF80 0028			H'1F80 0028			16
        // Synchronous DRAM mode registers
        // For area 2								SDMR2			W		—						H'FF90 xxxx*		H'1F90 xxxx			8
        // For area 3								SDMR3											H'FF94 xxxx*		H'1F94 xxxx

        #endregion
        #region cache

        // Name 									Abbreviation	R/W		Initial Value*1			P4 Address*2		Area 7 Address*2	Access Size
        // Cache control register					CCR				R/W		$00000000				$FF00001C			$1F00001C			32
        // Queue address control register 0			QACR0			R/W		Undefined				$FF000038			$1F000038			32
        // Queue address control register 1			QACR1			R/W		Undefined				$FF00003C			$1F00003C			32

        #endregion
        #region dma controller

        // Channel		Name								Abbreviation    Read/Write	Initial Value	P4 Address		Area 7 Address		Access Size
        // 0			DMA source address register 0		SAR0			R/W*2		Undefined		H'FFA00000		H'1FA00000			32
        //				DMA destination address register 0	DAR0			R/W*2		Undefined		H'FFA00004		H'1FA00004			32
        //				DMA transfer count register 0		DMATCR0			R/W*2		Undefined		H'FFA00008		H'1FA00008			32
        //				DMA channel control register 0		CHCR0			R/W*1,*2	H'00000000		H'FFA0000C		H'1FA0000C			32
        // 1			DMA source address register 1		SAR1			R/W			Undefined		H'FFA00010		H'1FA00010			32
        //				DMA destination address register 1	DAR1			R/W			Undefined		H'FFA00014		H'1FA00014			32
        //				DMA transfer count register 1		DMATCR1			R/W			Undefined		H'FFA00018		H'1FA00018			32
        //				DMA channel control register 1		CHCR1 			R/W*1		H'00000000		H'FFA0001C		H'1FA0001C			32
        // 2			DMA source address register 2		SAR2			R/W			Undefined		H'FFA00020		H'1FA00020			32
        //				DMA destination address register 2	DAR2			R/W			Undefined		H'FFA00024		H'1FA00024			32
        //				DMA transfer count register 2		DMATCR2			R/W			Undefined		H'FFA00028		H'1FA00028			32
        //				DMA channel control register 2		CHCR2			R/W*1		H'00000000		H'FFA0002C		H'1FA0002C			32
        // 3			DMA source address register 3		SAR3			R/W			Undefined		H'FFA00030		H'1FA00030			32
        //				DMA destination address register 3	DAR3			R/W			Undefined		H'FFA00034		H'1FA00034			32
        //				DMA transfer count register 3		DMATCR3			R/W			Undefined		H'FFA00038		H'1FA00038			32
        //				DMA channel control register 3		CHCR3			R/W*1		H'00000000		H'FFA0003C		H'1FA0003C			32
        // Common		DMA operation register				DMAOR			R/W*1		H'00000000		H'FFA00040		H'1FA00040			32

        #endregion
        #region exception

        // Name										Abbreviation	R/W			Initial Value*1			P4 Address*2		Area 7 Address*2	Access Size
        // TRAPA exception register					TRA				R/W			Undefined				$FF000020			$1F000020			32
        // Exception event register					EXPEVT			R/W			$00000000/$00000020*1	$FF000024			$1F000024			32
        // Interrupt event register					INTEVT			R/W			Undefined				$FF000028			$1F000028			32

        #endregion
        #region hitachi user debug interface

        //											CPU Side -----------------------------------------+
        //																									Hitachi-UDI Side-------------------------+
        // Name						Abbreviation	R/W     P4 Address	Area 7 Address		Access Size		R/W			Access Size		Initial Value*
        // Instruction register 	SDIR			R		H'FFF00000	H'1FF00000			16				R/W			16				H'FFFF
        // Data register H			SDDR/SDDRH		R/W		H'FFF00008	H'1FF00008			32/16			—			32				Undefined
        // Data register L			SDDRL			R/W		H'FFF0000A	H'1FF0000A			16				—			—				Undefined
        // Bypass register			SDBPR			—		—			—					—				R/W			1				Undefined

        #endregion
        #region i/o

        // Name										Abbreviation	R/W			Initial Value*			P4 Address			Area 7 Address		Access Size
        // Port control register A					PCTRA			R/W			H'00000000				H'FF80002C			H'1F80002C			32
        // Port data register A						PDTRA			R/W			Undefined				H'FF800030			H'1F800030			16
        // Port control register B					PCTRB			R/W			H'00000000				H'FF800040			H'1F800040			32
        // Port data register B						PDTRB			R/W			Undefined				H'FF800044			H'1F800044			16
        // GPIO interrupt control register			GPIOIC			R/W			H'00000000				H'FF800048			H'1F800048			16
        // Serial port register						SCSPTR1			R/W			Undefined				H'FFE0001C			H'1FE0001C			8
        // Serial port register						SCSPTR2			R/W			Undefined				H'FFE80020			H'1FE80020			16

        #endregion
        #region interrupt controller

        // Name										Abbreviation	R/W			Initial Value*1			P4 Address			Area 7 Address		Access Size
        // Interrupt control register				ICR				R/W			*2						H'FFD00000			H'1FD00000			16
        // Interrupt priority register A			IPRA			R/W			H'0000					H'FFD00004			H'1FD00004			16
        // Interrupt priority register B			IPRB			R/W			H'0000					H'FFD00008			H'1FD00008			16
        // Interrupt priority register C			IPRC			R/W			H'0000					H'FFD0000C			H'1FD0000C			16

        #endregion
        #region mmu

        // Name										Abbreviation	R/W			Initial Value*1			P4 Address*2		Area 7 Address*2	Access Size
        // Page table entry high register			PTEH			R/W			Undefined				$FF000000			$1F000000			32
        // Page table entry low register			PTEL			R/W			Undefined				$FF000004			$1F000004			32
        // Page table entry assistance register		PTEA			R/W			Undefined				$FF000034			$1F000034			32
        // Translation table base register			TTB				R/W			Undefined				$FF000008			$1F000008			32
        // TLB exception address register			TEA				R/W			Undefined				$FF00000C			$1F00000C			32
        // MMU control register						MMUCR			R/W			$00000000				$FF000010			$1F000010			32

        #endregion
        #region serial communication interface

        // Name										Abbreviation	R/W			Initial	Value			P4 Address			Area 7 Address		Access Size
        // Serial mode register						SCSMR1			R/W			H'00					H'FFE00000			H'1FE00000			8
        // Bit rate register						SCBRR1			R/W			H'FF					H'FFE00004			H'1FE00004			8
        // Serial control register					SCSCR1			R/W			H'00					H'FFE00008			H'1FE00008			8
        // Transmit data register					SCTDR1			R/W			H'FF					H'FFE0000C			H'1FE0000C			8
        // Serial status register					SCSSR1			R/(W)*1		H'84					H'FFE00010			H'1FE00010			8
        // Receive data register					SCRDR1			R			H'00					H'FFE00014			H'1FE00014			8
        // Serial port register						SCSPTR1			R/W			H'00*2					H'FFE0001C			H'1FE0001C			8

        #endregion
        #region serial communication interface fifo

        // Name										Abbreviation	R/W			Initial Value			P4 Address			Area 7 Address		Access Size
        // Serial mode register						SCSMR2			R/W			H'0000					H'FFE80000			H'1FE80000			16
        // Bit rate register						SCBRR2			R/W			H'FF					H'FFE80004			H'1FE80004			8
        // Serial control register					SCSCR2			R/W			H'0000					H'FFE80008			H'1FE80008			16
        // Transmit FIFO data register				SCFTDR2			W			Undefined				H'FFE8000C			H'1FE8000C			8
        // Serial status register					SCFSR2			R/(W)*1		H'0060					H'FFE80010			H'1FE80010			16
        // Receive FIFO data register				SCFRDR2			R			Undefined				H'FFE80014			H'1FE80014			8
        // FIFO control register					SCFCR2			R/W			H'0000					H'FFE80018			H'1FE80018			16
        // FIFO data count register					SCFDR2			R			H'0000					H'FFE8001C			H'1FE8001C			16
        // Serial port register						SCSPTR2			R/W			H'0000*2				H'FFE80020			H'1FE80020			16
        // Line status register						SCLSR2			R/(W)*3		H'0000					H'FFE80024			H'1FE80024			16

        #endregion
        #region smart card interface

        // Name										Abbreviation	R/W			Initial Value			P4 Address			Area 7 Address		Access Size
        // Serial mode register						SCSMR1			R/W			H'00					H'FFE00000			H'1FE00000			8
        // Bit rate register						SCBRR1			R/W			H'FF					H'FFE00004			H'1FE00004			8
        // Serial control register					SCSCR1			R/W			H'00					H'FFE00008			H'1FE00008			8
        // Transmit data register					SCTDR1			R/W			H'FF					H'FFE0000C			H'1FE0000C			8
        // Serial status register					SCSSR1			R/(W)*1		H'84					H'FFE00010			H'1FE00010			8
        // Receive data register					SCRDR1			R			H'00					H'FFE00014			H'1FE00014			8
        // Smart card mode register					SCSCMR1 		R/W			H'00					H'FFE00018			H'1FE00018			8
        // Serial port register						SCSPTR1 		R/W			H'00*2					H'FFE0001C			H'1FE0001C			8

        #endregion
        #region timer

        // Channel		Name							Abbreviation	R/W		Power-On Reset	Manual Reset	Standby Mode	Initial Value	P4 Address		Area 7 Address		Access Size
        // Common		Timer output control register	TOCR			R/W		Initialized		Initialized		Held			H'00			H’FFD80000		H'1FD80000			8
        //				Timer start register			TSTR			R/W		Initialized		Initialized		Initialized*1	H'00			H’FFD80004		H'1FD80004			8
        // 0			Timer constant register 0		TCOR0			R/W 	Initialized		Initialized		Held			H'FFFFFFFF		H’FFD80008		H'1FD80008			32
        //				Timer counter 0					TCNT0			R/W 	Initialized		Initialized		Held*2			H'FFFFFFFF		H’FFD8000C		H'1FD8000C			32
        //				Timer control register 0		TCR0			R/W 	Initialized		Initialized		Held			H'0000			H’FFD80010		H'1FD80010			16
        // 1			Timer constant register 1		TCOR1			R/W 	Initialized		Initialized		Held			H'FFFFFFFF		H’FFD80014		H'1FD80014			32
        //				Timer counter 1					TCNT1			R/W 	Initialized		Initialized		Held*2			H'FFFFFFFF		H’FFD80018		H'1FD80018			32
        //				Timer control register 1		TCR1			R/W 	Initialized		Initialized		Held			H'0000			H’FFD8001C		H'1FD8001C			16
        // 2			Timer constant register 2		TCOR2			R/W 	Initialized		Initialized		Held			H'FFFFFFFF		H’FFD80020		H'1FD80020			32
        //				Timer counter 2					TCNT2			R/W 	Initialized		Initialized		Held*2			H'FFFFFFFF		H’FFD80024		H'1FD80024			32
        //				Timer control register 2		TCR2			R/W 	Initialized		Initialized		Held			H'0000			H’FFD80028		H'1FD80028			16
        //				Input capture register			TCPR2			R		Held			Held			Held			Undefined		H’FFD8002C		H'1FD8002C			32

        #endregion
        #region user break controller

        // Name								Abbreviation	R/W		Initial Value	P4 Address		Area 7 Address		Access Size
        // Break address register A			BARA			R/W		Undefined		H'FF200000		H'1F200000			32
        // Break address mask register A	BAMRA			R/W		Undefined		H'FF200004		H'1F200004			8
        // Break bus cycle register A		BBRA			R/W		H'0000			H'FF200008		H'1F200008			16
        // Break ASID register A			BASRA			R/W		Undefined		H'FF000014		H'1F000014			8
        // Break address register B			BARB			R/W		Undefined		H'FF20000C		H'1F20000C			32
        // Break address mask register B	BAMRB			R/W		Undefined		H'FF200010		H'1F200010			8
        // Break bus cycle register B		BBRB			R/W		H'0000			H'FF200014		H'1F200014			16
        // Break ASID register B			BASRB			R/W		Undefined		H'FF000018		H'1F000018			8
        // Break data register B			BDRB			R/W		Undefined		H'FF200018		H'1F200018			32
        // Break data mask register B		BDMRB			R/W		Undefined		H'FF20001C		H'1F20001C			32
        // Break control register			BRCR			R/W		H'0000*			H'FF200020		H'1F200020			16

        #endregion
    }
}
