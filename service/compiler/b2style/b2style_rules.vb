
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'nlexer_rule syntaxer_rule
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_rules
    Public ReadOnly nlexer_rule() As Byte
    Public ReadOnly syntaxer_rule() As Byte

    Sub New()
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(904), _
        "H4sIAAAAAAAEAF1UzW7bMAy+6ynYBCs2x2ruXZIe1g0YUOy0nWyvUCzaESJLriQ3LdY92Q57pL3CKNlJfw4m+VEU9Yki/e/PXzaH7zvloVEa4aC0hi1Cbc09uoASlAkWHHo7uBpz8BbmX799uvlx/RmkRQ/GBjhYt7+gPF+s1vagTAsSG2VUUNZ4EC7m6xUla5ztYOvDo8ZlbSW2aJZG4wO6WzdovAgP4YLtD1w1oJpooPYIUUTQWAf0RfOwS2yjjFBaIhMNbW0PUUTgMAzOwKiiY+tQ7CHJMbhVNSTJ2FYLs4ei3FYLAtZqKIIbMG8EnV4xKgOxdVAscl5dFaWksK1qB/JDQoXObyo2NNqK0ZNdjFE+uFiRWVHO8uxpVmUzxmrbdQLKnAxtDVyyuwF9rBbvhNvDFW0SLvBeONE60e/gF0MjX+DfU8TWiXqPAd6n9SP6MK36u4GKf3KXRYp6662Yx07xkcpHxoT3qjUd0s3WjF71WjUNugjjG3nGSHhuHUdKpGG1Zi1VNKB79m3WbDTWa0YNMnnP1uPWsBMGVqddCW7oXCmhXLBOmcEDZ92gg+r1I5QZk+peSYQl66yEd6y3B3qKn/QAgQsj4TxZ1B3lE0v4nCVAMLbnGV1QN1yZmvLTCQlJrIFzItRQnXaqCbBaMafa3RFuiFKKHHmtRzCxO6JnjpPnyPQYEPlO9sR6QifuL3AiPeHxHhOYbjOhV5wn32vm9G7U30Z06HtRI5ys5KdC6IFIzieDTZofVNjx9B84LqV5WBXZ06ZabMbNjaHhpoCk0/DFWacto05TayRN8DwpKiK1v0auaZHHxo+dtFxSztJU2fjMbxbLLK5myyojkU4Njz3GYyfNWLwRjdkhv80vL6tolHK0M/YfXWU8WNUEAAA="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(1292), _
        "H4sIAAAAAAAEALVWzW7cIBC+8xQouUUmrxD1EFW9JFWSS2VZEbbxLloMLuCs8vYd/gxee9se2osN3wwwfDPMDPr29en55fH97cf3x1fcCipPFTZcHgQjgktGOjWOTNoKj7OwfIWhl+fnt7hSK2WJ/ZwYQrf4SWFj9dxZbOZpUtreI7Qo4HqYZWe5khX+oGJmpGcDl9wh5MztkRg2cjhDOA1JR2Ym2jFSYS47Mfeswn4B/PkgYUjOmk4T6yss1IF32z3cqV5vLWgQSpb4Y8LHWKotaTXtTszieqKajoIb2zxgJvtFENgwwAKTYJxTO4AZR7QsiGvDqY4x2txhD6FLQYCxHwdhNgihP5G0YRFnGUKZQHw6k2VWXnYxHteLl8BWd918L1RopWtXV2loUMKy9tYxK4ma7co515bh+vK+OY46AQaUEACzATs1s7OWy5QfpNKMaGbAfpKigHRUiMrR1GpGTzGenJliIOv9YkhBUBRc/+Y6uO6U7KO15yMXsMegNBFKTRCH12jcOijpbP2z4aCIoU2E5HinxgAb7jkHylB50SsqG0KCXu1x2veRspHL2aSxu+EkPuO05x/cveQgU2nFpM5Mx3HLLaGyL2YqiTK8QIINwPeRDzYCmh+OEWmi2WjxgfMxHy7euldaPfKCdyaMy3txffOA1oDb0CG4cIh3sxOEwV8fBmkpRoZbDOPLnHQ12rODV4EZV+xhS0BvXHqR7y4DLa33jzMqJTNS+Ee4QVvVP7GBND2TeNDF8/ygmtMWypOLOVcULDu4oGn5YeauUM2DUBT+rVKgDoUIqllpQ2EaJLx0Tmk8URMLzK5sX1Bcz5LqzwykLVq+hzdoVx0KBOSgzR6TMna7w44qrqVK0Q6lMb0s1qV4v8WvcFnRpyq8u3Xkvt7ZBu3qF57ZW7M5dJ+TdKzPFjFR5ByR0oPPDDEpLPkgpQI/cQPBjCH2SOEdHCBxW6bjzAvgLbGfMxVZmJH4Ax4Dkpi7YnNx9X9jd85ZRbr6HxdCq1cTG4vfVEG8muVGKQj3GqXwTradEsqCVc5IHVFZdIqOKNAcOiK0qt4uKQZgyWuu53wcW9ZjGmq2M84yX7F6rllnnXMEP7EovoFEATo9pAJM30OKuEFBBtunPRyMdpvKdEzRaMXuNGr6tb7MBDjtlqb1jvrS4QZwgKoBvcG6yQ2VyyGx5pYdmytCUJAGFBpkN4+j4PBb/EUIdfbdyx22CrcMA6NAmwHmghH3KLXLoVkPWTZl0ShzO6fhSr2coCt9d1Iq2fsFlVydHYcMAAA="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
