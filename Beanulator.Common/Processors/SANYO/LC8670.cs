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

using System.Runtime.InteropServices;

namespace Beanulator.Common.Processors.SANYO
{
    /// <summary>
    /// Sanyo LC8670 "Potato", Sega Dreamcast VMU Processor
    /// Hardware Information: http://mc.pp.se/dc/vms/
    /// </summary>
    public abstract class LC8670
    {
        // Addr	Name	Description
        private const int SFR_ACC   = 0x100; // 100	ACC		Accumulator
        private const int SFR_PSW   = 0x101; // 101	PSW		Processor Status Word
        private const int SFR_B     = 0x102; // 102	B		B Register (general purpose)
        private const int SFR_C     = 0x103; // 103	C		C Register (general purpose)
        private const int SFR_TRL   = 0x104; // 104	TRL		Table Reference (Low byte)
        private const int SFR_TRH   = 0x105; // 105	TRH		Table Reference (High byte)
        private const int SFR_SP    = 0x106; // 106	SP		Stack Pointer
        private const int SFR_PCON  = 0x107; // 107	PCON	Power Control register
        private const int SFR_IE    = 0x108; // 108	IE		Interrupt Enable control
        private const int SFR_IP    = 0x109; // 109	IP		Interrupt Priority Ranking control

        private const int SFR_EXT   = 0x10D; // 10D	EXT		External Memory control
        private const int SFR_OCR   = 0x10E; // 10E	OCR		Oscillation Control Register (32kHz/600kHz/6MHz)

        private const int SFR_T0CON = 0x110; // 110	T0CON	Timer/Counter 0 control
        private const int SFR_T0PRR = 0x111; // 111	T0PRR	Timer 0 Prescaler Data Register
        private const int SFR_T0L   = 0x112; // 112	T0L		Timer 0 Low
        private const int SFR_T0LR  = 0x113; // 113	T0LR	Timer 0 Low Reload Register
        private const int SFR_T0H   = 0x114; // 114	T0H		Timer 0 High
        private const int SFR_T0HR  = 0x115; // 115	T0HR	Timer 0 High Reload Register

        private const int SFR_T1CNT = 0x118; // 118	T1CNT	Timer 1 control

        private const int SFR_T1LC  = 0x11A; // 11A	T1LC	Timer 1 Low Compare Data Register
        private const int SFR_T1L   = 0x11B; // 11B	T1L		Timer 1 Low (Read only)
        private const int SFR_T1LR  = 0x11B; // 11B	T1LR	Timer 1 Low Reload Register (Write only)
        private const int SFR_T1HC  = 0x11C; // 11C	T1HC	Timer 1 High Compare Data Register
        private const int SFR_T1H   = 0x11D; // 11D	T1H		Timer 1 High (Read only)
        private const int SFR_T1HR  = 0x11D; // 11D	T1HR	Timer 1 High Reload Register (Write only)

        private const int SFR_MCR   = 0x120; // 120	MCR		Mode Control Register

        private const int SFR_STAD  = 0x122; // 122	STAD	Start Addresss Register
        private const int SFR_CNR   = 0x123; // 123	CNR		Character Number Register
        private const int SFR_TDR   = 0x124; // 124	TDR		Time Division Register
        private const int SFR_XBNK  = 0x125; // 125	XBNK	Bank Address Register

        private const int SFR_VCCR  = 0x127; // 127	VCCR	LCD Contrast Control Register

        private const int SFR_SCON0 = 0x130; // 130	SCON0	SIO0 Control Register
        private const int SFR_SBUF0 = 0x131; // 131	SBUF0	SIO0 Buffer
        private const int SFR_SBR   = 0x132; // 132	SBR		SIO Baud Rate Generator Register

        private const int SFR_SCON1 = 0x134; // 134	SCON1	SIO1 Control Register
        private const int SFR_SBUF1 = 0x135; // 135	SBUF1	SIO1 Buffer

        private const int SFR_P1    = 0x144; // 144	P1		Port 1 Latch
        private const int SFR_P1DDR = 0x145; // 145	P1DDR	Port 1 Data Direction Register
        private const int SFR_P1FCR = 0x146; // 146	P1FCR	Port 1 Function Control Register

        private const int SFR_P3    = 0x14C; // 14C	P3		Port 3 Latch
        private const int SFR_P3DDR = 0x14D; // 14D	P3DDR	Port 3 Data Direction Register
        private const int SFR_P3INT = 0x14E; // 14E	P3INT	Port 3 Interrupt Control Register

        private const int SFR_P7    = 0x15C; // 15C	P7		Port 7 Latch
        private const int SFR_I01CR = 0x15D; // 15D	I01CR	External Interrupt 0, 1 control
        private const int SFR_I23CR = 0x15E; // 15E	I23CR	External Interrupt 2, 3 control
        private const int SFR_ISL   = 0x15F; // 15F	ISL		Input Signal Selection Register

        private const int SFR_VSEL   = 0x163; // 163	VSEL	VMS Control Register
        private const int SFR_VRMAD1 = 0x164; // 164	VRMAD1	Work RAM Access Address 1
        private const int SFR_VRMAD2 = 0x165; // 165	VRMAD2	Work RAM Access Address 2
        private const int SFR_VTRBF  = 0x166; // 166	VTRBF	Send/Receive Buffer
        private const int SFR_VLREG  = 0x167; // 167	VLREG	Length registration

        private const int SFR_BTCR   = 0x17F; // 17F	BTCR	Base Timer Control Register

        private Flags flags;
        private Registers registers;
        private byte[] ram; // memory, 256  bytes
        private byte[] sfr; // special function registers
        private byte code;

        //		0			1			2, 3		4-7				8-F
        // 0	NOP			BR r8		LD d9		LD @Ri			CALL a12
        // 1	CALLR r16	BRF r16		ST d9		ST @Ri
        // 2	CALLF a16	JMPF a16	MOV #i8,d9	MOV #i8,@Ri		JMP a12
        // 3	MUL			BE #i8,r8	BE d9,r8	BE @Ri,#i8,r8
        // 4	DIV			BNE #i8,r8	BNE d9,r8	BNE @Ri,#i8,r8	BPC d9,b3,r8
        // 5	---			---			DBNZ d9,r8	DBNZ @Ri,r8
        // 6	PUSH d9					INC d9		INC @Ri			BP d9,b3,r8
        // 7	POP d9					DEC d9		DEC @Ri
        // 8	BZ r8		ADD #i8		ADD d9		ADD @Ri			BN d9,b3,r8
        // 9	BNZ r8		ADDC #i8	ADDC d9		ADDC @Ri
        // A	RET			SUB #i8		SUB d9		SUB @Ri			NOT1 d9,b3
        // B	RETI		SUBC #i8	SUBC d9		SUBC @Ri
        // C	ROR			LDC			XCH d9		XCH @Ri			CLR1 d9,b3
        // D	RORC		OR #i8		OR d9		OR @Ri
        // E	ROL			AND #i8		AND d9		AND @Ri			SET1 d9,b3
        // F	ROLC		XOR #i8		XOR d9		XOR @Ri

        #region modes

        #endregion
        #region codes

        #endregion

        protected abstract byte read(ushort address);
        protected abstract void write(ushort address, byte data);

        public struct Flags
        {
            public int cy;      // carry (carry from bit 7)
            public int ac;      // auxiliary carry (carry from bit 3)
            public int irbk1;   // indirect register bank 1
            public int irbk0;   // indirect register bank 0
            public int ov;      // arithmetic overflow
            public int rambk0;  // ram bank
            public int p;       // parity
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Registers
        {
            [FieldOffset(0)] public byte pcl;
            [FieldOffset(1)] public byte pch;
            [FieldOffset(2)] public byte eal;
            [FieldOffset(3)] public byte eah;

            [FieldOffset(0)] public ushort pc;
            [FieldOffset(2)] public ushort ea;
        }
    }
}
