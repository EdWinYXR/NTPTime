# NTPTimeDemo
            NTPClient nTP = new NTPClient("10.10.10.241");
            
            var time = nTP.GetUtc();//ͨ��udp�ӿڻ�ȡʱ��

            nTP.SetLocalDateTime(time);//����ϵͳʱ��