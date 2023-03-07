﻿using Io.HcxProtocol.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestCore.Dto
{
    [TestClass]
    public class JsonRequestTest
    {

        [TestMethod]
        public void JsonRequestUnitTest()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("payload", "eyJ4LWhjeC1yZWNpcGllbnRfY29kZSI6InRlc3RwYXlvcjEuaWNpY2lAc3dhc3RoLWhjeC1kZXYiLCJ4LWhjeC10aW1lc3RhbXAiOiIyMDIyLTEwLTE5VDE1OjM1OjQyKzA1MzAiLCJ4LWhjeC1zZW5kZXJfY29kZSI6InRlc3Rwcm92aWRlcjEuYXBvbGxvQHN3YXN0aC1oY3gtZGV2IiwieC1oY3gtY29ycmVsYXRpb25faWQiOiJkMGJmNjI2MS1lNzZlLTQ5NGMtYWNkYS0wYWJhM2IyMTZiZGYiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAtMjU2IiwieC1oY3gtYXBpX2NhbGxfaWQiOiJiYTEwM2UxNi1jNzY5LTQzNzYtYTkyMy0wN2NmZDBmYzRhYjIifQ.LI1jbKtjwX9mZ_wKs_VLByvkJEQFk5aV70Fa3gYZzt6Kfo12PY0aC9IvTSxzthyipekmw4iLds7hE3Fkhi60ZZp0QxfG7uVo03fyIVPl_F_niurT8ZBDZJrn6fHlrJN5yIc6npjQmNVJwAuN7J2XdAIfvcZlaBrLWIYbN-0Ad94CXg3I_xlfcjuWJZbWNT0jaKM4ylMHFCc9DyKxKOKGDhjQrbIE27IQsX_sMc51IdLoN-_FHSLKXW-o0hH6sqitk3MJi-HKUYA64TK7PlAyo5ywognB9XjtOtp2Vpu97E_OzVRdfRG1SXxqTqMCQN5jK5BbuWO68Ef_yGXiQUp6ow.nvijE_pgCWyILykv.oxL2WcjICM5o42HHehghBB3e-PF9Trp202YfM8aawta0BPk82-hk1k_gdG2PECJtXuUBSo9QykvY-xP5TPGumIeR1EAZTrxd5aQQ3-_eG9c9A2O_i3FlJ0vx2bHmYI6k-tIS6PzqVxp2Q86gFQH8hcXldEaOdWt30NScbN6Uk-qEzML_SjsAST1dwzpBuRqYHLGM7pJiS9EvSA50xuLcAlzzwPBXKUBHVdz-B6FnLN3yE4D_t6YO6nAeJuxikWdFrvaMWsfA5Pb56QQY7TRPTjRlHRSvRpcuzL40dowOz85SdBXv5lyGB09yLfdbynq-CBVfxWk1iWoIR8pjQHYhl-qz8LfpsatJMmeZRrTWY2kRFeTD6-TeWMjXyjSp83J2H9YbAjbqnIHqKRJcpEpz6JVzZNxasF3GtSGP5whs2eMT3R38qQtQpyRK1UDDZkkRutpZ0iWE8yFoPIU80FP6_CKJ203oQINpzJTB83ExgF5m399fTHu_B-o-tMNCOFzUZS90EAzCMOzeUaV-Cr96jniSyzmOAJnEo7I-9BtJWmJX2CCJ2lw9aE08mK1SQRSyxNxuMrEP9eVrNgr2aPNWDbxdu2Pqi6SXIEjAtd-4o5VNXQ2E9KecU0XaG40E8vLfPNnQZQpZcrrwfHzq1uJ3t6f1FeZF4gTsOXWzvZoPIul04UBg1_yFBkJ78SuStMs22-YM0CWb0as5yR_vqdNbbgtphsXdlUuI9IfnS2Gr73zafwWSCTRE015EB31d4xAhq4NEtVOnZC-9yuvjAEfsVGHZAvXB1S--9ri9mwfxoQsOVkQHFVOaW-fsrIqkhkZzA6KyLR3wh4a8lYUNh51db4Ob72t8x6oSqfvQlbJIIFtvt3rcrW444Syx2tqOsDQthMnA2NFIn9RJusYmsoxjG5Dx5O-yIPqPNXhEkVvpFOIYLYRZHPhfP87FxelkHNhG9v3WIuU0MGev9KxI7gIQj_Op6HkHJLCBiIEh0YvEYxB1j1HDGNaRkqq3W89IpndXTFA7wY4mzziKLDd1S7gj8YtppbvwqYGgiuPvQ-KO7Eh0qw2M-xhCiwxLHEO8aFTfk6kvLf9Agyrz3vU9jqwUjbQFoQ1BrCcDwqPY7Gdas5782jq0vnToU1YlcRwh2kTPzslNNqWS3mVlVLcpQc4GltN-_AI09B5ZAnkeouCh5h-3V_hkYFpeUg1X92UlwxkcODm52iUx5xC0_Yq3Bqd6I_inp0O0Brag-Bk_f1d0POdwLjDERSlAHDhi_MsoGCR_HYrvHePdxcOca6kJ1hZwh1dVVbGmBdpIyQEr_qJQ3f0Y6Ir9BUwxcFf4F4xodJDHyFmjRCB4zxKsoxCUfqwlV5hFyNCEUMC8TJ2qpK5nx4qOWm7waRBtwOdyBUJxzyiqXiq23sr07pKeC2GTl_Nl7FNLEiwKd-uGQ53fG4XR5YFGv5CJVqjVU0S-JjApWSgENm3kBm23pU5a-gcI6q9Aztgv15Axr-mX4e3oAsZRjpv1Xp1uPjGdI2iK5MUT0eDu5vZmx1IyC6IArfJnhINE52baT8Wai6Dk9mxEGo6URsKCx-xtFCam1KoJb9akdh1Xwd9LJD4ybdZiinXWkDEXoX6iF89htXgzB7IPaf4UwoTLykVHBiOztOe-iIT1cVGsi__bAe_tcUvxgC-0q36qdql3Bk1tnvAZ5drD9zHom_G7ej4NXLHQGF6JPQBSzuzxlwNINTqhsjV2nsEUUqpaZbJ1ouH10WQQooTR1z0ElRZWoL5Qo71h_pL7CaBOwr_Db8DF9wkXCzII4jib0_5W4MehMnsy4Ogrg3XrJ1ZRDFwESGrNiziOVdi_rR6g3Mu-8J4kwG0_QkNxgEisdVtDLLZX9-xs_4e7QgIyjpgoOn8VZ06nokKuNea4hLyQETfcuG1Yyw9sdmUik39XhJooHNErhSeytLumw5Ks9L6tN9NZB-CQlTBNhMvLOOhhWhxiNphc9myvAucJXWaf67VSw00jjy-bbi722LcriO4eG4I3BZBbRWaCT4BcNyOxjr1PXAiWttJdyuM9sZmI6yPpezhDQp-wTqaOEYXT6NIL-4eblxxL2mftiD6yamQbPaaFeoWIthTvrJQTvRGKlXbAbrWFzbTia-r2d_qkE2el5w-85h1_U3063GLRVTA-s-5MwO53_RlduIZvYKght-J1W9MhengloIjulHxthveoBQXp2pia0TpiVDNRbkSND5bUBIRjUT7_e3wwPLY_08DCPvIvJa3pwHbMZmnzOW2edu3Lo3bKG9lyB4PpLwsc2oqX_yZ2jqop5vgOZwFZwDl035b64NN7cQPebGHrsJMrXgQOpw6e2hA9Sr42_DFcXMtECoVjUHbwy-HEBW2wqDnLdCvD6NJ0V-ig6He41fi_1MSKP1QgNmeOl2zYNi2D62xecv_I_30zIDGqdRJtO4kP28J3jWt9Tvx6S3cteUH5debxJ3R9C9TBkS98OBkqYmQjIZNyHxDz0d8CDkgxOqHOr1tMeykJPBAdd1fvAcCDKClohISoOTjA2rFcjqe0Fl8A7ga_87W8YYW4tSOPlUIMBcVwNSHNFQs9XIaefy59cCThyAz648pnVGQlf_DV7PrD_MStOROkueMAbeSR4TRgzUnQPJsm5RcfQtLJG5omg0Ku3yC8gOTinr3zb8b6LNf_Zt0k3o4IzQ4yKvHDTnpp7lRQlY5izL2Wwdjd56i6_gpdq45buZl-sVIwnKzrOmjqkIdnm7NafuqJ9hNhfmhOXdhm7YTvVqAMy46lFYJGBDkcsgLPADx2GlAcYoMsXE2NA0lozK8oKrx6L2xtOv-SfARl_AHveyy8QmU0JXIJGFp_pUMJjv5JJSqL25yUeppeQhEjIQF1OKVc2uHP2h5oZdXgNb-jTa-0lSVukgQGfIAEFalHLdeB508ngNyZDOnHRoLirQV7R-F8ow-IOKJBDZMZaYKEJaOYtyvu9Sy0PsRji1CZS3Y39v0Xmh6sDXUOuFT8mZL2BvNAWPLabVFTeHzCLyARlf96XGYB6WuloOt2ku_kNyoLdEgBYTUog02YSRsctHlKsA1E3dD9DSEKj2nYT67lZQOoRPKGmhW2Rr9f_xsVlkHSuiTozY2ouxPQN0nGcLqeFdZYSvPGY0Mn59D8LcKsGj1umD-DISbeeaHSjU2XTHPhSMXjZbQ1t9cIqU7UeALXaPtMSnP4aGUnaSOHBLd4g6Mqo41_NkdX-2vhx2Gxi7hPtcWqk1N8KIfugV_Eo7LpCOkAZF-8yNRtKLMkVHJUkNKXZ1fb2eXR7IaVxUBJs_1dnMs0tt-jK-ubz8ZcqvXd4Rm_uMyXkVZAdMHbx002JDqsnB_JHETmgaORxHyzGohVHnwzbdYnC_Lmi8kbbbBfW31_Sry0qN-WuUY8tOptOZHLxlfIwx13TEKkUdWy3nKQdz3u9ck_L9ah3ax_6cKtZgM8EyHMNKJtrNsGnJNFI1pSPzjKp2iHZGSGL_lOtPXM5maAReaiv7LVGJ6MwmaA1OVsP2-AzZ1UX-4yGWvWjgjORB0qCklhcM72CuDAnUC3J20wQwfIAnyzvfrGIJCcjexFelcxGe4kttS1_UKYTINnUZ4tHvUnIU35mEcHSHwVwxo9HRBFM4EjLDMu1I1yG7tSeIUuj0dwv3NDlfNjjy3kk9zOKb470S0ss0_pmMMNk1UDnyxahou8vxMussLTvSe0kJ5t_ZrrQOxVKKX7aAnV5oZeptvKWk66S8LrjV596MVnZZnWc4klEQ0L145TdP1TPd_tQdEUPAFa4gSmeijWinHva0EVITETIzdqUy6pZ8f87XBRc2_3afMs2SFirEpqe3gKM4-vN4f7D6X4fc4eSuAj3rh5VtDiNgyr-cKfuxsj0F-477K2cyKy2TVttwoWvYb3Q3LtLSIoAI1D5LtXDOdpLNafcrHYm-fcLM6DfrwdpmDRvwOZu0WTaq5eBNf7lgCvsDbCAHGAO_ow3SgNrKDCgMh2YJh4Ig7DorRJypz1-3qcIn4sI9cVrCAma5-8C-bllQIKM-TqJBhWfNys4CyfV3HCVTZRR_6kButDiq78OeTYfKGwWE4U2k8kGdb_WsLgO3JSU2meXf2LdgE-DJUJ28CmiyGF-hmw3384MxpSHSNcDW8kEoNR1sw7t6QZI8u5z4k66NHFWmw0OwyLk62lEKwgpgJ-VcEWlhsBNEwCoRYVg1bfp7VYeLUcxP9XTC9OLxqj42y0s9hYmMdI9LLBt13SSQzJ54m_l4y9osKNL6p2hBJV-XP3gA4adE9Hhx12c1mPjDBLy6s4zUPBXCvJi-uPb0xrKtLdzUpJQ6ra9kLi5xKJmgKqSc-g6Iw7rSkV652vcvq8zlUU1quTUmNfXK7H-gAGHvRq1CHMaUDQeA33YwAmbBei9vhWKEiLoFuKLsDEP0V3uwLFQ8A_FHbmU8mLO5rP2Dsr097renXHSEUQ5FM_1zups6-28qdeY1XxGu69DUxaPc7KtElgJdE1IMEF56kZiEHKbaSOzxPvsmLCJEjMAqsDyisIZf8xV7-VQj88of0OQo6Lw_4syJ0_Gtj2rEEEgGLxqCU8I4UDeqmGuJuM9GBwAAva9-af87HQ5cVOW2AnuDQmJxKthc57Kurh6HXVa35fej-7aBusFzlMGZPkfhSIMab6wgMH4HGSASgDp5vxYCcHaNVELEf8p5sR84OvZ_sbky8SuHsM9HCegg12EjD_PlU204ZbDGBogerHTYH7aP2OOD_kY970buu5qVw_EghCRN0lSgtqlLB-Cz764SLroQavvTnSUAA9U71ej9UlqkWFFxvw_jr8B68WblRUU0uTlN9UxCTfRlCDjb3cCLAdmKVxnOYBCI4mfZkVIwx2g6mtKhhJijQrJ-UdoU-zZ5xt3A6xqTqomr3_ZxY23AS7fPW7JpFHiejev9I9bZ3aUxbSxVxwd_IBCLxZVTvgRGi5dg.6IXejsgAX-PvS20muMwD-A");
            JSONRequest jsonRequest = new JSONRequest(payload);
            Assert.AreEqual(payload, jsonRequest.GetPayload());
        }

    }
}
