
Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _status
    'http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html

    'http://tools.ietf.org/html/rfc2518#section-10
    'http://tools.ietf.org/html/rfc4918#section-21.4
    'http://tools.ietf.org/html/rfc5842#section-7.1

    'http://tools.ietf.org/html/rfc3229#section-10.4.1

    'http://www.iana.org/assignments/http-status-codes/http-status-codes.xhtml

    'http://en.wikipedia.org/wiki/List_of_HTTP_status_codes

    Sub New()
        assert(HttpStatusCode.Continue = 100)
        assert(HttpStatusCode.SwitchingProtocols = 101)
        assert(HttpStatusCode.OK = 200)
        assert(HttpStatusCode.Created = 201)
        assert(HttpStatusCode.Accepted = 202)
        assert(HttpStatusCode.NonAuthoritativeInformation = 203)
        assert(HttpStatusCode.NoContent = 204)
        assert(HttpStatusCode.ResetContent = 205)
        assert(HttpStatusCode.PartialContent = 206)
        assert(HttpStatusCode.MultipleChoices = 300)
        assert(HttpStatusCode.MovedPermanently = 301)
        assert(HttpStatusCode.Found = 302)
        assert(HttpStatusCode.SeeOther = 303)
        assert(HttpStatusCode.NotModified = 304)
        assert(HttpStatusCode.UseProxy = 305)
        assert(HttpStatusCode.TemporaryRedirect = 307)
        assert(HttpStatusCode.BadRequest = 400)
        assert(HttpStatusCode.Unauthorized = 401)
        assert(HttpStatusCode.PaymentRequired = 402)
        assert(HttpStatusCode.Forbidden = 403)
        assert(HttpStatusCode.NotFound = 404)
        assert(HttpStatusCode.MethodNotAllowed = 405)
        assert(HttpStatusCode.NotAcceptable = 406)
        assert(HttpStatusCode.ProxyAuthenticationRequired = 407)
        assert(HttpStatusCode.RequestTimeout = 408)
        assert(HttpStatusCode.Conflict = 409)
        assert(HttpStatusCode.Gone = 410)
        assert(HttpStatusCode.LengthRequired = 411)
        assert(HttpStatusCode.PreconditionFailed = 412)
        assert(HttpStatusCode.RequestEntityTooLarge = 413)
        assert(HttpStatusCode.RequestUriTooLong = 414)
        assert(HttpStatusCode.UnsupportedMediaType = 415)
        assert(HttpStatusCode.RequestedRangeNotSatisfiable = 416)
        assert(HttpStatusCode.ExpectationFailed = 417)
        assert(HttpStatusCode.InternalServerError = 500)
        assert(HttpStatusCode.NotImplemented = 501)
        assert(HttpStatusCode.BadGateway = 502)
        assert(HttpStatusCode.ServiceUnavailable = 503)
        assert(HttpStatusCode.GatewayTimeout = 504)
        assert(HttpStatusCode.HttpVersionNotSupported = 505)
    End Sub

    <Extension()> Private Function in_range(ByVal this As HttpStatusCode,
                                            ByVal min As UInt32,
                                            ByVal max As UInt32) As Boolean
        Return this >= min AndAlso this < max
    End Function

    <Extension()> Private Function in_x_range(ByVal this As HttpStatusCode,
                                              ByVal x As UInt32) As Boolean
        Return this.in_range(x * 100, (x + 1) * 100)
    End Function

    <Extension()> Public Function informational(ByVal this As HttpStatusCode) As Boolean
        Return this.in_x_range(1)
    End Function

    <Extension()> Public Function successful(ByVal this As HttpStatusCode) As Boolean
        Return this.in_x_range(2)
    End Function

    <Extension()> Public Function redirection(ByVal this As HttpStatusCode) As Boolean
        Return this.in_x_range(3)
    End Function

    <Extension()> Public Function client_error(ByVal this As HttpStatusCode) As Boolean
        Return this.in_x_range(4)
    End Function

    <Extension()> Public Function server_error(ByVal this As HttpStatusCode) As Boolean
        Return this.in_x_range(5)
    End Function

    'http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html

    <Extension()> Public Function [continue](ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Continue
    End Function

    <Extension()> Public Function switching_protocols(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.SwitchingProtocols
    End Function

    <Extension()> Public Function ok(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.OK
    End Function

    <Extension()> Public Function created(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Created
    End Function

    <Extension()> Public Function accepted(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Accepted
    End Function

    <Extension()> Public Function non_authoritative_information(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.NonAuthoritativeInformation
    End Function

    <Extension()> Public Function no_content(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.NoContent
    End Function

    <Extension()> Public Function reset_content(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.ResetContent
    End Function

    <Extension()> Public Function partial_content(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.PartialContent
    End Function

    <Extension()> Public Function multiple_choices(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.MultipleChoices
    End Function

    <Extension()> Public Function moved_permanently(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.MovedPermanently
    End Function

    <Extension()> Public Function found(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Found
    End Function

    <Extension()> Public Function see_other(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.SeeOther
    End Function

    <Extension()> Public Function not_modified(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.NotModified
    End Function

    <Extension()> Public Function use_proxy(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.UseProxy
    End Function

    <Extension()> Public Function switch_proxy(ByVal this As HttpStatusCode) As Boolean
        Return this = 306
    End Function

    <Extension()> Public Function temporary_redirect(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.TemporaryRedirect
    End Function

    <Extension()> Public Function bad_request(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.BadRequest
    End Function

    <Extension()> Public Function unauthorized(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Unauthorized
    End Function

    <Extension()> Public Function payment_required(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.PaymentRequired
    End Function

    <Extension()> Public Function forbidden(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Forbidden
    End Function

    <Extension()> Public Function not_found(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.NotFound
    End Function

    <Extension()> Public Function method_not_allowed(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.MethodNotAllowed
    End Function

    <Extension()> Public Function not_acceptable(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.NotAcceptable
    End Function

    <Extension()> Public Function proxy_authentication_required(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.ProxyAuthenticationRequired
    End Function

    <Extension()> Public Function request_timeout(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.RequestTimeout
    End Function

    <Extension()> Public Function conflict(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Conflict
    End Function

    <Extension()> Public Function gone(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.Gone
    End Function

    <Extension()> Public Function length_required(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.LengthRequired
    End Function

    <Extension()> Public Function precondition_failed(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.PreconditionFailed
    End Function

    <Extension()> Public Function request_entity_too_large(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.RequestEntityTooLarge
    End Function

    <Extension()> Public Function request_uri_too_long(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.RequestUriTooLong
    End Function

    <Extension()> Public Function unsupported_media_type(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.UnsupportedMediaType
    End Function

    <Extension()> Public Function requested_range_not_satisfiable(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.RequestedRangeNotSatisfiable
    End Function

    <Extension()> Public Function expectation_failed(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.ExpectationFailed
    End Function

    <Extension()> Public Function internal_server_error(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.InternalServerError
    End Function

    <Extension()> Public Function not_implemented(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.NotImplemented
    End Function

    <Extension()> Public Function bad_gateway(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.BadGateway
    End Function

    <Extension()> Public Function service_unavailable(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.ServiceUnavailable
    End Function

    <Extension()> Public Function gateway_timeout(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.GatewayTimeout
    End Function

    <Extension()> Public Function http_version_not_supported(ByVal this As HttpStatusCode) As Boolean
        Return this = HttpStatusCode.HttpVersionNotSupported
    End Function

    'http://tools.ietf.org/html/rfc2518#section-10
    'http://tools.ietf.org/html/rfc4918#section-21.4
    'webdav

    <Extension()> Public Function processing(ByVal this As HttpStatusCode) As Boolean
        Return this = 102
    End Function

    <Extension()> Public Function multi_status(ByVal this As HttpStatusCode) As Boolean
        Return this = 207
    End Function

    <Extension()> Public Function already_reported(ByVal this As HttpStatusCode) As Boolean
        Return this = 208
    End Function

    <Extension()> Public Function unprecessable_entity(ByVal this As HttpStatusCode) As Boolean
        Return this = 422
    End Function

    <Extension()> Public Function locked(ByVal this As HttpStatusCode) As Boolean
        Return this = 423
    End Function

    <Extension()> Public Function failed_dependency(ByVal this As HttpStatusCode) As Boolean
        Return this = 424
    End Function

    <Extension()> Public Function insufficient_storage(ByVal this As HttpStatusCode) As Boolean
        Return this = 507
    End Function

    'http://tools.ietf.org/html/rfc3229#section-10.4.1

    <Extension()> Public Function im_used(ByVal this As HttpStatusCode) As Boolean
        Return this = 226
    End Function

    'other

    <Extension()> Public Function method_failure(ByVal this As HttpStatusCode) As Boolean
        Return this = 424 OrElse this = 420
    End Function

    <Extension()> Public Function i_m_a_teapot(ByVal this As HttpStatusCode) As Boolean
        Return this = 418
    End Function

    <Extension()> Public Function authentication_timeout(ByVal this As HttpStatusCode) As Boolean
        Return this = 419
    End Function

    <Extension()> Public Function enhance_your_calm(ByVal this As HttpStatusCode) As Boolean
        Return this = 420
    End Function

    <Extension()> Public Function unordered_collection(ByVal this As HttpStatusCode) As Boolean
        Return this = 425
    End Function

    <Extension()> Public Function upgrade_required(ByVal this As HttpStatusCode) As Boolean
        Return this = 426
    End Function

    <Extension()> Public Function precondition_required(ByVal this As HttpStatusCode) As Boolean
        Return this = 428
    End Function

    <Extension()> Public Function too_many_requests(ByVal this As HttpStatusCode) As Boolean
        Return this = 429
    End Function

    <Extension()> Public Function request_header_fields_too_large(ByVal this As HttpStatusCode) As Boolean
        Return this = 431
    End Function

    <Extension()> Public Function login_timeout(ByVal this As HttpStatusCode) As Boolean
        Return this = 440
    End Function

    <Extension()> Public Function no_response(ByVal this As HttpStatusCode) As Boolean
        Return this = 444
    End Function

    <Extension()> Public Function retry_with(ByVal this As HttpStatusCode) As Boolean
        Return this = 449
    End Function

    <Extension()> Public Function blocked_by_windows_parental_controls(ByVal this As HttpStatusCode) As Boolean
        Return this = 450
    End Function

    <Extension()> Public Function unavailable_for_legal_reasons(ByVal this As HttpStatusCode) As Boolean
        Return this = 451
    End Function

    <Extension()> Public Function redirect(ByVal this As HttpStatusCode) As Boolean
        Return this = 451
    End Function

    <Extension()> Public Function request_header_too_larget(ByVal this As HttpStatusCode) As Boolean
        Return this = 494
    End Function

    <Extension()> Public Function cert_error(ByVal this As HttpStatusCode) As Boolean
        Return this = 495
    End Function

    <Extension()> Public Function no_cert(ByVal this As HttpStatusCode) As Boolean
        Return this = 496
    End Function

    <Extension()> Public Function http_to_https(ByVal this As HttpStatusCode) As Boolean
        Return this = 497
    End Function

    <Extension()> Public Function client_closed_request(ByVal this As HttpStatusCode) As Boolean
        Return this = 499
    End Function

    <Extension()> Public Function loop_detected(ByVal this As HttpStatusCode) As Boolean
        Return this = 508
    End Function

    <Extension()> Public Function bandwidth_limit_exceeded(ByVal this As HttpStatusCode) As Boolean
        Return this = 509
    End Function

    <Extension()> Public Function not_extended(ByVal this As HttpStatusCode) As Boolean
        Return this = 510
    End Function

    <Extension()> Public Function network_authentication_required(ByVal this As HttpStatusCode) As Boolean
        Return this = 511
    End Function

    <Extension()> Public Function origin_error(ByVal this As HttpStatusCode) As Boolean
        Return this = 520
    End Function

    <Extension()> Public Function connection_timed_out(ByVal this As HttpStatusCode) As Boolean
        Return this = 522
    End Function

    <Extension()> Public Function proxy_declined_request(ByVal this As HttpStatusCode) As Boolean
        Return this = 523
    End Function

    <Extension()> Public Function a_timeout_occurred(ByVal this As HttpStatusCode) As Boolean
        Return this = 524
    End Function

    <Extension()> Public Function network_read_timeout_error(ByVal this As HttpStatusCode) As Boolean
        Return this = 598
    End Function

    <Extension()> Public Function network_connect_timeout_error(ByVal this As HttpStatusCode) As Boolean
        Return this = 599
    End Function
End Module
