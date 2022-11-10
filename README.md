# NTPTimeDemo
            NTPClient nTP = new NTPClient("10.10.10.241");
            
            var time = nTP.GetUtc();//通过udp接口获取时间

            nTP.SetLocalDateTime(time);//更改系统时间