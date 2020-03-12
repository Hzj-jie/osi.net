
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'prefix ufloat
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_statements
    Public ReadOnly prefix() As Byte
    Public ReadOnly ufloat() As Byte

    Sub New()
        prefix = Convert.FromBase64String(strcat_hint(CUInt(1684), _
        "H4sIAAAAAAAEAL1ay47bNhRdZ4D5B8IrC3CAmXTQBDACeNVVl90LskVPaGgkV6QSu8V8WRf9pP5C+ZT4NmnLwSxGli7POTyXpC9F//fPv48PmPSofQXbT5icG1iWpB9gSW+Cr2DxB71erN2YfdXgMeg39oFFPT5871A9RWFSl91AlrI1KsDfjw8ANN0r2oEFagns++FIAI2jYQCBzebYwz06Qbwh8O2IN6Ilw373w1OEFHh6lQ2/7bpm7AL7MDKgPViqa+B213GyWLPIdwCpT5dbjd7KZnFxzIBUcTw2W5zRKkmc1zS/r6iI5/YCBAuZIGjKp5jd0PewJeUbXipn6OMe4qEha+84mVqERopsvuDte0iGvp0QuQR7otDP5a5rd9U0C1ZAXh3MAVsdj7Ct6TA9GPgoC3p7JvBWYNIx7CWHUtbLGKwDeJWswJQPnrwxqmprmU4qk/0/mCNWoh50Cj7c/HBdn4LGRvla3T64ty/ztJ1/9lstgQUtkNDrYAxKaq28t2T3FaSdkVi7ptPWOrshf6i3YjfiVEZn4Z9D1SwVPbNWXip3ebg7iXg7ed8ZaOYMcdy9D6mRPxETGJeCXti6Avz/wTTLtseXzWLlzdahCLBOHU9i1nyag11ASbt1q39KjxN4Z+7vaw8rusyX5FvVXjHS3roeXjO6DdqkPJtCZ+96gvPzK2ggxtc6z9pe4/zEmWS7JnHeHicYfg/urr+8qto6Oq3M0zqwog1WzsrB7iYJSLL/PtQJ3s9FrCbNrb6bk/V2Gdnuzy0gMwfZ9GbBs0ekRK0oeviTk1kJ7wZS4gbtIDjRP63Ip9UrJlVL8IYqLZ/8jzD6C5bdnsMbC9HJuwGRYlStdxcpNMSvhBfvthR2U5T1dxHDkBN8oYsY80RNCyWF6zqvvarOM1p09lvEllZmz8+Q5Bg1abIL/6quY2uJ2uHVbHsX3OCZk4RBeteFKJh3jgW23ozBM/Gz8MV3nz9X3BS+Ow1seJMYRLqLoPVvqB1wgvl42JK+2pH0DAjkWA4imDmJEEThVGTQWPlwzKKVGDo25wS/VGiGXwo8ZlkENssyxRV2LYPpgms1+o5qGC2L5We3MhZtVW0cemEVr5lNm6WagMnj26/bRXjzYe3RfT55UjKnN77k2W8OnHHfRdfni+mLv2jMSh+TckXuMhVk547Jyk3cjZpSEnfsfsA+YbXicelLlYCNrVMhwJxFSrCEV6hUjgvL05ZG8be2lwuQNqMAUbDRIiQO6JUZKTniaN6Odymjo8sYGhI01us4nE9iuM9xLKfLGDasVt45dblWU0U3B8/+0t/0YGTx7c4yKbwT5uSfMCPv7T3zzaFTeA5x5hoGfR1LsJnMZVQ+c6/hyXaYkc/Ux7jN9pGVNHc8ulITVzuycV8jPP1ejOekknXxJES8O2deP76hBoLwmwodjBsDvrpbXuM7cQWeWSOK/gHpwarYEc/ZYwOLb35W4OULf4T1R9o5nE7NHbH3uduiWAFcTL2VDuCQxZq9QWttUz49v3x++fLLry+fNXN0xXKDYkQ6hoigKUIeMZpBfM9JcYQrMnPs1wkfWUY/xI8sg+PI+0K2cF3LBFAeW2f6su14fG6NYk289ZMBizWJgZ+uxxiMc/9UBl1/mvYcVH7cn6Q3BTWq0+jIhR8pRAH0nyj8DyElH8lwIwAA"))

        assert(prefix.ungzip(prefix))
        ufloat = Convert.FromBase64String(strcat_hint(CUInt(528), _
        "H4sIAAAAAAAEALVTXW+CMBR9N+E/NDxpYnjY45Yl/JMG6BWvKZS1t0yz7L+v0iqKsiFmb5Sej3tO22iRKyVZ/mLoIIFz+LCZXNqNVBkxXLPwtVuxr2jBWAfWYKykt+NaqhILFncgzw27DNku7iAayOq6J31Hi2hgWiv6P+NLl6XHrO6PUWrICDSnbVbPnKRSGuY0cLJW+q8ihqmUXt4P4Ki71Xp4st3fkfwSjHkm/JE/J3zn+1zyfvQpsYPwGZMJMW57FdEhXbRhOBwTrrC2ZqK0sTnprKCH9F2n2MjDRIsT/BELgS0KGDcIy7E74enTbsXQulGfoCdG67C/5TKksS57cS/GSXG3czYJ0gFcAW2V4HVWAXtnsTKYGNAtFpBg7R5a40xAJ43GCglbSJyIAME9z6zZCOM1x5JbAUWwjy9763DaNsSOakHrapQ0dTIb3INJCarGpH5crxJGvz2PXhf2UFgCfjWta85z7h0QCyW2CkVfoSHBlaVheTf7I53j6UH+AKldPmSEBgAA"))

        assert(ufloat.ungzip(ufloat))
    End Sub
End Module
