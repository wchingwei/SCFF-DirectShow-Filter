DEL ..\tmp\ffmpeg_test.flv
"..\..\ext\ffmpeg\amd64\bin\ffmpeg.exe" -rtbufsize 100MB -r 30 -f dshow -s 640x360 -i video="SCFF DirectShow Filter":audio="Mixer (Creative SB X-Fi)" -r 30 -s 640x360 -vcodec libx264 -preset medium -x264opts b-adapt=2:direct=auto:keyint=300:me=umh:rc-lookahead=50:ref=6:subme=5 -maxrate 1200k -bufsize 2400k -crf 23 -qmin 10 -qmax 51 -acodec libvo_aacenc -ar 44100 -ab 96k -ac 2 -async 100 -threads 4 -f flv "..\tmp\ffmpeg_test.flv"