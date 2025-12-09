using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.Expressions.Tests
{
    public class FunctionTest : TestBase
    {
        
        public FunctionTest() : base()
        {
            testItems = new List<TestItem>
            {
                new TestItem("${f:toupper:09Az啊 ⏎}", "09AZ啊 \r\n"),
                new TestItem("${f:tolower:09Az啊 ⏎}", "09az啊 \r\n"),
                new TestItem("${f:tofullwidth:09Az,.:;啊 ⏎}", "０９Ａｚ，。：；啊　\r\n"),
                new TestItem("${f:tohalfwidth:０９Ａｚ，。：；啊　⏎}", "09Az,.:;啊 \r\n"),
                new TestItem("${f:tosimpcn:Aaa洛可利亞⏎⏎奏鳴曲}", "Aaa洛可利亚\r\n\r\n奏鸣曲"),
                new TestItem("${f:totradcn:Aaa洛可利亚奏鸣曲}", "Aaa洛可利亞奏鳴曲"),

                new TestItem("${f:toblackchar:09AZ}", "？？"),//?
                new TestItem("${f:toblackchar(true):}", "？？"),//?
                new TestItem("${f:towhitechar:0123456789abc}", "？？abc"),//?

                new TestItem("${f:length:ABCDE}", "5"),
                new TestItem("${f:length: a }", "3"),
                new TestItem("${f:length:}", "0"),
                new TestItem("${f:length:\r\n\r\na\r\rb\n\nc⏎⏎d  e}", "15"),

                new TestItem("${f:hex2dec:FF}", "255"),
                new TestItem("${f:hex2float:3F800000}", "1"),
                new TestItem("${f:hex2double:3FF0000000000000}", "1"),

                new TestItem("${f:parsedmg:ABCD}", "0"),

                new TestItem("${f:float2hex:1}", "0000803F"),
                new TestItem("${f:double2hex:1}", "3FF0000000000000"),

                new TestItem("${f:dec2hex:255}", "FF"),
                new TestItem("${f:dec2hex2:15}", "0F"),
                new TestItem("${f:dec2hex4:15}", "000F"),
                new TestItem("${f:dec2hex8:15}", "0000000F"),

                new TestItem("${f:ord:AB}", "65,66"),

                new TestItem("${f:ord(|):😀}", "128512"),
                new TestItem("${f:chr:65,66}", "AB"),

                new TestItem("${f:chr(|):128512|65}", "😀A"),


                new TestItem("${f:padleft(0,5):123}", "00123"),
                new TestItem("${f:padright(46,5):abc}", "abc.."),

                new TestItem("${f:repeat(3):ab}", "ababab"),
                new TestItem("${f:repeat(2,-):ab}", "ab-ab"),
                new TestItem("${f:repeat(0):ab}", ""),

                new TestItem("${f:repeat(-2):abc}", "cbacba"),


                new TestItem("${f:replace(aa):baabaa}", "bb"),
                new TestItem("${f:replace(aa,b):aaaa}", "bb"),
                new TestItem("${f:replace(aa,,true):aaaa}", ""),
                new TestItem("${f:dictreplace(a=b,c=d):1a2c3}", "1b2d3"),

                new TestItem("${f:substring(1):ABCDE}", "BCDE"),
                new TestItem("${f:substring(1,3):ABCDE}", "BCD"),
                new TestItem("${f:substring(-2):ABCDE}", "DE"),

                new TestItem("${f:slice:A}", "A"),

                new TestItem("${f:pick(0):a,b,c}", "a"),
                new TestItem("${f:pick(-1):a,b,c}", "c"),
                new TestItem("${f:pick(1,;):a;b;c}", "b"),

                new TestItem("${f:args(1,2,3):}", "(1)\n(2)\n(3)"),

                new TestItem("${f:i(a):banana}", "1"),
                new TestItem("${f:lastindexof(a):banana}", "5"),

                new TestItem("${f:indicesof(aba):ababa}", "0,2"),
                new TestItem("${f:indicesof(aba,|):ababa}", "0|2"),

                new TestItem("${f:compare(ABC):abc}", "0"),

                new TestItem("${f:compare(abc,false):abc}", "0"),

                new TestItem("${f:versioncompare(1.2.0.0):1.1.8.0}", "-1"),

                new TestItem("${f:contain(bc):abcde}", "1"),
                new TestItem("${f:startwith(ab):abcde}", "1"),
                new TestItem("${f:endwith(de):abcde}", "1"),
                new TestItem("${f:equal(abc):abc}", "1"),

                new TestItem("${f:ifcontain(bc,YES,NO):abcde}", "YES"),
                new TestItem("${f:ifstartwith(ab,YES,NO):abcde}", "YES"),
                new TestItem("${f:ifendwith(de,YES,NO):abcde}", "YES"),
                new TestItem("${f:ifequal(abc,YES,NO):abc}", "YES"),

                new TestItem("${f:match(a):a}", "1"),

                new TestItem("${f:capture(ab,1):(a)(b)}", "a"),

                new TestItem("${f:capture(abc,name):(?<name>abc)}", "abc"),

                new TestItem("${f:ifmatch(abc,YES,NO):^a}", "YES"),

                new TestItem("${f:trim(*):***abc***}", "abc"),
                new TestItem("${f:trimleft(*):***abc}", "abc"),

                new TestItem("${f:trimright(42):abc***}", "abc"),

                new TestItem("${f:format(System.Int32,X4):1234}", "04D2"),

                new TestItem("${f:utctime(yyyy-MM-dd HH:mm:ss):0}", "1970-01-01 00:00:00"),
                new TestItem("${f:localtime(yyyy-MM-dd HH:mm:ss):0}", "1970-01-01 08:00:00"),
            };
        }
    }
}
