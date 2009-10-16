using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace AIMHealth.Safari.Logging
{
    public class SimpleLoggerConfig : ConfigurationSection
    {
        [ConfigurationProperty("fileNameConfig",
                               DefaultValue = @"c:\SafNet\Logs\log4net.xml")]
        public string SimpleLoggerConfigFile
        {
            get
            {
                return (string)this["fileNameConfig"];
            }
            set
            {
                this["fileNameConfig"] = value;
            }
        }

        [ConfigurationProperty("fileNameFormat",
                               DefaultValue= @"c:\SafNet\Logs\{0}.txt")]
        public string SimpleLoggerFileNameFormat
        {
            get
            {
                return (string)this["fileNameFormat"];
            }
            set
            {
                this["fileNameFormat"] = value;
            }
        }

        
        [ConfigurationProperty("patternFormat",
                               DefaultValue="%newline--------%newline%date [%thread] %-5level %logger - %message%newline")]
        public string PatternFormat
        {
            get
            {
                return (string)this["patternFormat"];
            }
            set
            {
                this["patternFormat"] = value;
            }
        }


        [ConfigurationProperty("rollingFileMaxFileSize",
                               DefaultValue="10000000")]
        public int RollingFileMaxFileSize
        {
            get
            {
                return (int)this["rollingFileMaxFileSize"];
            }
            set
            {
                this["rollingFileMaxFileSize"] = value;
            }
        }


        [ConfigurationProperty("rollingFileMaxRollBackups",
                               DefaultValue="2")]
        public int RollingFileMaxRollBackups
        {
            get
            {
                return (int)this["rollingFileMaxRollBackups"];
            }
            set
            {
                this["rollingFileMaxRollBackups"] = value;
            }
        }
    }
}
