using System;
using System.Collections.Generic;
using System.Text;
using AYNETTEK.UHFReader;

namespace LineGolden_PLasma
{
    public class MyTag
    {
        public DateTime GottenTime;//Read time
        public byte[] AntReadMark = new byte[8];
        public int ReadCount { get; set; }
        public int AntPort { get; set; }
        public UInt16 TagIndex { get; set;}
        public UInt32 frequency { get; set; }
        private string _epc { get; set; }
        public string Epc { get { return _epc; } }
        public string Rssi { get; set; }

        public MyTag(AutoReadEventArgs tag)
        {
            //AntMark((int)tag.ant);
            this.ReadCount = tag.readCnt;
            this.frequency = tag.freq;
            this._epc = tag.EPC;
            this.Rssi = tag.RSSI;
            this.GottenTime = tag.time;
        }

        public MyTag(TagReadData tag)
        {
//            AntMark((int)tag.ant);
            this.AntPort = tag.ant;
            this.ReadCount = tag.readCnt;
            this.frequency = tag.freq;
            this._epc = tag.EPC;
            this.Rssi = tag.RSSI;
            this.GottenTime = tag.time;
            this.TagIndex = tag.TagIndex;
        }

/*        public void AntMark(int ant)
        {
            if (ant <= 8)
            {
                this.AntPort = ant;   
                AntReadMark[ant-1] = 1;
                
                
            }
        }*/

        public void TagIndexRefresh(ushort TagIndex)
        {
            this.TagIndex = TagIndex;
        }
    }
    class MyTagList
    {
        public List<MyTag> mylist = new List<MyTag>();

        public int myFind(string TagString)//Compare the EPC, return the index if found, return -1 if not found
        {
            for (int i = 0; i < mylist.Count; i++)
            {
                if (mylist[i].Epc.Equals(TagString))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
