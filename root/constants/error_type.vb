﻿
Public Enum error_type
    first                   'begin of errorType
    application             'for information around application
    critical                'for application error, cannot be handled, will cause stop or out of service
    exclamation             'for application error, can be handled, may cause more serious issue
    information             'for normal information
    system                  'for system error, network/file IO or other
    warning                 'for application error, can be handled, will not cause more serious issue
    user                    'for user input issue, such as in c/s model or b/s model, c/b sends wrong data type
    performance             'for performance information output
    other                   'for other output
    last                    'last of errorType
End Enum
