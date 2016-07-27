
the service starts with a configuration file as the only command line parameter.
if no configuration file name provided, it will try to use the tcp_bridge.ini instead.
but, if the configuration file cannot be opened or loaded, the service would stop.

the configuration file contains several sections, named as
[connection1], [connection2] ... [connectionN]
each section supports following parameters
    name, the name of the connection, no default value
    type, if the connection is outgoing or incoming, outgoing means it's an outgoing connection, otherwise incoming
    token, the token used to generate the connection, default value is empty, means no token
           do not set this value if the connection is not connected to another tcp-bridge
    host, the host name for outgoing connections
    port, the port for outgoing connections, or the listen port for incoming connections
    connecting_timeout_ms, the timeout in milliseconds to generate an outgoing connection, default is 8000
    send_rate_sec, the count of bytes send in one second, 0 or negative number means no limitation, default is 1
                   lower bound, if the speed is less than this ratio, the connection will be treated as dead
    response_timeout_ms, the timeout before receiving first byte from the connection, negative value means no limitation, default is 30000.
                         if the service bridging is not 'talk' mode, such as remote desktop,
                         consider to change it to a negative value or use socket transfer.
    receive_rate_sec, the count of bytes receive in one second, default is 1, others are same as send_rate_sec
    max_connecting, max count of half-open connections, default is 2
    max_connected, max count of connected connections,
                   default is 4096 for an outgoing connection, no default limitation for incoming connections
    no_delay, true means do not use Nagle algorithm, default is false
              this configuration only take effect if using stream based transfer,
              when using socket, the value is always true
    max_lifetime_ms, the idle time in milliseconds before the connection has been dropped, default is 24 hours
    ipv4, whether it's ipv4, default is true
    target, the target connection name to send the data to, default is empty
    reset_connection, whether actively reset the connection even the transfer ends correctly
    chunk_count, the count of buffers for the transfer, each piece of buffer is 8K by default
                 <same as default socket setting in windows>
                 set to 0 means do not use buffered transfer, set to any negative number means no limitation
                 set to other positive number means only use such count of buffers
                 default value is 0
    **stream_based is not supported anymore from this version**
    stream_based, whether use .net NetworkStream to handle the transfer, otherwise use Socket
                  NetworkStream will give a better performance in responding time,
                  but the timeout control is not friendly,
                  if send_rate_sec and receive_rate_sec is large,
                  the NetworkStream will pin cached data in the memory, and may cause a memory leak
                  and if a read / write operation is timeouted, the service can only shutdown the connection,
                  since there is no way to know how many data has been sent or read from the stream.
                  so usually you should set the a small send_rate_sec and receive_rate_sec
                  to make the connection live for longer, if enabled stream based transfer
                  but this will cause the connection to be pinned almost forever
                  the total QPS would be almost the same
                  default value is false
    enable_keepalive, whether use tcp level keepalive, default is true
    first_keepalive_ms, the milliseconds before the first keepalive package would be sent, default is 8000.
    keepalive_interval_ms, the milliseconds between two keepalive packages, default is 8000.
    for rdp, do not try to use buffered transfer, i.e. chunk_count = 0 only,
             it's due to the incompatibility between the buffered transfer and rdp protocol

the environment values can also impact the behavior of the service,
but do not try to use them unless fully understand the meaning and impact.
    tcp_trace / network_trace
        if there is an environment value named as tcp_trace or network_trace, no matter the value,
        service will log more network information, such as connection generated, dropped, into the log file.

    priority
        the value can be set as 64 or Idle, 16384 or BelowNormal, 32 or Normal, 32768 or AboveNormal, 128 or High, 256 or RealTime.
        service will use this priority. case sensitive.

    processor_affinity
        the FIRST processor the threads of the service are affine.
        say the threadpool contains 2 threads, queue_runner contains 1 thread, while the affinity has been set to 1,
        queue_runner thread will be affine to processor 1,
        1st threadpool thread will be affine to processor 2, and the other is processor 3.

    queue_runner_thread_count
        queue_runner threads are a set of threads to detect the no callback events, such as socket poll.
        by default, the system has max(1, processor_count / 4) threads to run as queue_runner threads.
        if set queue_runner_thread_count in the range of (0, processor_count],
        system will use this value as the queue_runner threads count.
    
    thread_count
        thread_count controls the threadpool thread count, by default the value is same as processor count.

    busy_wait
        if there is an environment value named as busy_wait, no matter the value,
        both queue_runner threads and threadpool threads will not use OS wait event to wait for next valid job,
        instead they will directly loop.
        using busy_wait will improve the performance, especial for the response delay.
        but the system will use all the processor resource assigned to it.
        but thread_count, queue_runner_thread_count and processor_affinity are also needed to make the improve happen.

    for an eight core machine, meanwhile running some other services on it, a common combination of the environments is,
    queue_runner_thread_count=1
    thread_count=3
    busy_wait=1
    processor_affinity=4
    which will use 4, 5, 6, 7 cores FULLY, while other threads in the system will be on the other 4 cores, i.e. GC.
    process level affinity can also be set manually, but the impact will be very limited.
