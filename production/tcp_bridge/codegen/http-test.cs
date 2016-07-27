
using System;
using System.IO;
using System.Text;

public static class http_test
{
    private static int id = 0;
    private static int port = 700;

    private static string ts(object i)
    {
        return Convert.ToString(i);
    }

    private static string strcat(params object[] inputs)
    {
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < inputs.Length; i++)
        {
            sb.Append(ts(inputs[i]));
        }
        return ts(sb);
    }

    private static void wl(string s)
    {
        Console.WriteLine(s);
    }

    private static void wl()
    {
        Console.WriteLine();
    }

    private static void write_section()
    {
        wl(strcat("[connection", ++id, "]"));
    }

    private static void write_next_port()
    {
        wl(strcat("port=", port++));
    }

    private static void write_last_port()
    {
        wl(strcat("port=", port - 1));
    }

    public static void Main()
    {
        wl("; for ((;;)); do for i in $(cat url-list); do if [ -n \"$i\" ]; then wget \"$i\" -S -O temp.txt --read-timeout=2; cat temp.txt; fi; done; done");
        write_section();
        wl(strcat("name=to-server\n",
                  "type=outgoing\n",
                  "port=800\n",
                  "host=localhost\n",
                  "max_connecting=128\n",
                  "max_connected=2048\n",
                  "send_rate_sec=2048\n",
                  "receive_rate_sec=2048"));
        wl();

        write_section();
        wl(strcat("type=incoming\n",
                  "max_lifetime_ms=2000\n",
                  "max_connecting=128\n",
                  "max_connected=2048\n",
                  "chunk_count=0\n",
                  "response_timeout_ms=2000\n",
                  "target=to-server"));
        write_next_port();
        wl();

        write_section();
        wl(strcat("type=incoming\n",
                  "max_lifetime_ms=2000\n",
                  "max_connecting=128\n",
                  "max_connected=2048\n",
                  "chunk_count=-1\n",
                  "response_timeout_ms=2000\n",
                  "target=to-server"));
        write_next_port();
        wl();

        for(int chunk_selection = 0; chunk_selection <= 1; chunk_selection++)
        {
            for(int zip_selection = 0; zip_selection <= 2; zip_selection++)
            {
                for(int enc_selection = 0; enc_selection <= 2; enc_selection++)
                {
                    write_section();
                    wl(strcat("type=incoming\n",
                              "max_lifetime_ms=2000\n",
                              "max_connecting=128\n",
                              "max_connected=2048\n",
                              "chunk_count=", (chunk_selection == 0 ? "0" : "-1"), "\n",
                              "response_timeout_ms=2000\n",
                              "target=", chunk_selection, "_", zip_selection, "_", enc_selection));
                    write_next_port();
                    wl();

                    write_section();
                    wl(strcat("type=incoming\n",
                              "max_connecting=128\n",
                              "max_connected=2048\n",
                              "response_timeout_ms=2000\n",
                              "name=", chunk_selection, "_", zip_selection, "_", enc_selection, "\n",
                              "zipper=", (zip_selection == 0 ? "bypass" : (zip_selection == 1 ? "gzip" : "deflate")), "\n",
                              "encryptor=", (enc_selection == 0 ? "bypass" : (enc_selection == 1 ? "xor" : "ring"))));
                    write_next_port();
                    wl();

                    write_section();
                    wl(strcat("type=outgoing\n",
                              "host=localhost\n",
                              "max_connecting=4\n",
                              "max_connected=64\n",
                              "target=to-server\n",
                              "response_timeout_ms=2000\n",
                              "zipper=", (zip_selection == 0 ? "bypass" : (zip_selection == 1 ? "gzip" : "deflate")), "\n",
                              "encryptor=", (enc_selection == 0 ? "bypass" : (enc_selection == 1 ? "xor" : "ring"))));
                    write_last_port();
                    wl();
                }
            }
        }
    }
}

