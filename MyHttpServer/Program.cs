using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MyHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener httpListener = new HttpListener();

            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListener.Prefixes.Add("http://localhost:8810/");
            httpListener.Start();
            new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    HttpListenerContext httpListenerContext = httpListener.GetContext();
                    if (httpListenerContext.Request.RawUrl == "/")
                    {
                        httpListenerContext.Response.StatusCode = (int)HttpStatusCode.OK;// 200;
                        using (StreamWriter writer = new StreamWriter(httpListenerContext.Response.OutputStream))
                        {
                            writer.WriteLine("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/><title>升级包下载</title></head><body>");
                            writer.WriteLine("<div style=\"color:black;text-align:center;font-size:18px\"><p>升级包下载</p></div>");
                            writer.WriteLine("<hr>");
                            writer.WriteLine("<div style=\"color:black;text-align:center;font-size:10px\"><p>中间件程序升级包：<a href='/Upgrade.rar' >下载</a></p></div>");
                            writer.WriteLine("</body></html>");
                        }
                    }
                    else if (httpListenerContext.Request.RawUrl == "/Upgrade.rar")
                    {
                        httpListenerContext.Response.StatusCode = (int)HttpStatusCode.OK;// 200;
                        using (StreamWriter writer = new StreamWriter(httpListenerContext.Response.OutputStream))
                        {
                            try
                            {
                                FileStream fs = File.OpenRead(@"c:/Upgrade.rar"); //待下载的文件
                                CopyStream(fs, writer.BaseStream);
                            }
                            catch {
                                //writer.Close();
                            }
                        }
                    }
                    else {
                        httpListenerContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        httpListenerContext.Response.OutputStream.Close();
                    }
                }
            })).Start();
        }

        static void CopyStream(Stream orgStream, Stream desStream)
        {
            byte[] buffer = new byte[1024];

            int read = 0;
            while ((read = orgStream.Read(buffer, 0, 1024)) > 0)
            {
                desStream.Write(buffer, 0, read);

                //System.Threading.Thread.Sleep(1000); //模拟慢速设备
            }
        }
    }
}
