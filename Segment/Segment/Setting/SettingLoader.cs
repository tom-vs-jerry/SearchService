using System;
using System.Collections.Generic;
using System.Text;

namespace Segment.Setting
{
    public class SettingLoader
    {
        private void Load(string fileName)
        {
            SegmentSettings.Load(fileName);
        }

        public SettingLoader(string fileName)
        {
            Load(fileName);
        }

        public SettingLoader()
        {
            string fileName = Framework.Path.GetAssemblyPath() + "HBcomm.xml";
            Load(fileName);
        }
    }
}
