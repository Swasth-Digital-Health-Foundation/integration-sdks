import { HCXIntegrator } from "../src/hcx_integrator.js"; // adjust the path as necessary
import HcxOperations from "../src/utils/hcx_operations.js"; // adjust the path as necessary

async function validateHCXIntegratorOperations(config, fhirPayload) {
    const hcxIntegrator = new HCXIntegrator(config);
    const operation = HcxOperations.CLAIM_SUBMIT;

    try {
        const responseOutgoing = await hcxIntegrator.processOutgoing(
            fhirPayload,
            "testpayor1.swasthmock@swasth-hcx-staging",
            operation
        );

        const responseIncoming = await hcxIntegrator.processIncoming(
            responseOutgoing.payload,
            operation
        );

        const expectedOutgoing = {
            payload: 'eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZHQ00iLCJ4LWhjeC1hcGlfY2FsbF9pZCI6ImIwZWIyYTgxLTc1YWQtNGVmMy05MmY3LTEzOTM2MTgyNTA4NSIsIngtaGN4LXRpbWVzdGFtcCI6IjIwMjMtMDgtMjJUMTI6MDU6NTUuOTU5WiIsIngtaGN4LXdvcmtmbG93X2lkIjoiOTE2ZjE0ZDMtMGU1YS00NGVhLTlmMGYtMjM0NzU4ZGE3MTI0IiwieC1oY3gtc2VuZGVyX2NvZGUiOiJ0ZXN0cHJvdmlkZXIxLnN3YXN0aG1vY2tAc3dhc3RoLWhjeC1zdGFnaW5nIiwieC1oY3gtcmVjaXBpZW50X2NvZGUiOiJ0ZXN0cGF5b3IxLnN3YXN0aG1vY2tAc3dhc3RoLWhjeC1zdGFnaW5nIiwieC1oY3gtY29ycmVsYXRpb25faWQiOiJjN2QwYzZiMS01YTg2LTRjZDYtYTcyYi01ODY3Nzk1YTMzMWMiLCJraWQiOiJIaWJseW10VUl5dmVVOHA1Ujg2WXc1bDFVWHY0NklNRmIwcjI5MjBDVnR3In0.RvZzou7cWMXdPDCyx1pqXpyiMzljT0mu-ewmEzR61fsK95q3uSNM_p8ygqMoJQr4VzXeUAxWVMgv86pmzV-i1I4RvF5xetg_K8U56FXIzwRr-fNkIJ5DgUTAm53Nsxmx9re8oXYf2AbpH6us9zGvGYMwYGE-JR_vM_u98YCZgyH9Qg2dybJEbra8B9qswxhGUBzsQIH-UXQutttWwVH-yR7vKijNDzkFXEycoibUbW5qzvDthlHJX7J0McQGCs6GKrk_VD6YnRPypqSKcsqkNqDcKcDj4Lt34kyRB2X04vvOnOcHQgDHR7Um2iN8VL2kOCvCmyaussDREVyR5ZxD9w.sDaDfLzXVNUPg6nx.AzwfcxWSyUAYtCWpZM6kN3EDTMJ2gOFvO1Xc9wkfsJFkkigfO3cuZ6wo_fbe35fcycMvFTteVD6yx3wQonORiebflVezPjkIDJ_MwZTLd5OlJhuA7hQeeS2jwrlFl2a1NTMMMLk5mmt39IFYNHCLnxS2FZKmnRi3_oHd3JrCAckN-K9xzzdUo38Y27Yrogo4EvcHypWWriUa4WUISNZ7wRwcK0BZxBtU3bUzDPGE29R3TYuFAD8nE2bge4fncGsYcodUBBi6ZQVQqRB6XyDRjaNJ-J3ygzRhTfEHVaUyYk5Q2PiI9njEMfo6w0C920rijXcni-kbLV-kyaR1UilS_yJlBke0kwM0006yQC34SGJQX_zYny6SdRG2-7BGB3FC8Pfajj47Y_6cEbKhR0GZ75QstP15-o_cvHrdicpDNqF9swZGws-w6YgO5rD4-2U45MCnTcC2P1xtPPaxGpzPT4iWvoYWxRo3ecTXrO6tQEhp1mI23oPQdnFpO3bDtPAKC9Cql3ieefHgk4EhGg9KYnKyrmvZVCqfhN-1PUAw_5di93AY7OQZQR6XpEgrHGAic95UfO2wh9a9bRpGh1PHOwKlAzC7ngxrzVPN2AkBlKwJc-JnDfMDtIoevjQlTxGfF5HBkINesnx5ZdFGQ4ONl4mgmIuHImbtfX5A0yxpX769l8OfllDQXzHZENuK1K8qaZfnvcZR_Kj6v3qGQtCQ1HbaIUgZHAQlHFw8UhtnhiGSL8dAk4cE4q9V8YF_rDD8gOQlZe5TyIo62vLAhpRO4ag_DuwEaFvdNB1ZNo1t9eGGhSZIRaMYciSQm3OX_x3WMNf7jRtTzUtXxGaKFzwv7NhF0yq4mdoSnl78TXQS9SdLkjWe4WSi1O42C-0nJjwJTmpBCmWfu3z2-0aXyb0-SnKRsvaOw5dfbm2j-83Fkfay8n5JAzvKLRbLACnGphnhpfQ7ZTM6xkZhRFeBQ3WKZB8MveGc50rYgprA4hnMwAkIApw_CmDHXKSHskG8-XHi2pHJ4y2scLqIwPpoUtAJUWrVMIfBRw6yZJ-WMr49OZOZ4tTjTPY8Yfr4kRNKk9oHlsnXLnW2o2JYlhxDCrdwaAvTVqq23FgtD6ngzgqxbDrqNv83BliDNQ87emwe5nSdAWE2GgYUq3Bd5h0TBwa91qslJTZGFKxqaM3T7UGNZ-VlYWaVSheFwwfoNk5c41oI3RqIdhfWzjmDdAYMhSDXKQ5fSYAOU7F3krfJ7cq3C-upIN8zp0PJSHMMWWOL_D6T5K9ZWONz-W3IRSAKtk5c0wgGdVC831BrnoGMmgT0oLp_uK9xlqtquEPuCf0HYJ8mClLnhy_bsUoy5UifcDbOkxCmjD3RUKsH8b48KWnYB_G16DwE601EAkn3c_4F90hQ9KxvlKDHr_dc-4ysaPuMm0L1rk1Wvf19VWH1CCjkXrwAdHY9Ng-0tKH4tBtGyXeLe31Fg3Vf6faOJhvNH0OOXgFK7T-t8zBD-sYmHhzsP7RsyGCB30Dep2Foq7Urx-aWSqC_wuPK1a40_tQ3LIaQ-NFZyOl3_i5Ps9ebTrFgT9xBZo6c5xPT_XB1HSDguASUwX3DFLyvyrFsdKXKXyxhk8o9S5Gvsb4CS6-cJ2UREn-9qF-F_aGyeJuDoh2j8kdiJy4qUMeyvt4o5AfQAreG_SZL5ipl_mpl40GmA_r0Fep0HD2SfovY-KQUztHBPn7tDHX4PisBTlDKpKfd_EuOtP8FTP7V3c_vJ4fFGdfYnnKDcQnn1gRUOg-Ox0eJPwCdrnAwupepIbtEzUmBwGR9TURy8fjDbLbQDRFtZx5UWgjCTiC97kgSwOQ22weskl32RbWchR7qn_lScd7SkSheAiLV-cC9bpLiflmNkVsRlRfFngT4P9Jn4Jt5dgzEzpG6GyU3_eY1c42-Dlg05iQcPjEkqFnojggkKVPVCNYlJe2ELcodtX4ar6_TNzEuzJNsNFrttQm1upmhTsnr7QGhw7fH6bczF_7qBkZPGHZu25CnzFf1WqwgiCaO4Elsf6-xeW2apGVRQs3qJfGeMdH-1lXV10sV7zfZad7YPLT6vIW3A9kgrrH62n5GEHrpaAILuiEMjOYNjwVCuTskZeLvFsYI5uRBU58PCfkU0PIB50LQ_3-58iQKMpB7NpNWE4Uv5MHZiAKYlz4RP7EzQug-CEScOHdqGHQr0BZko0MKUSAszE1RSIIbvt5ptIt4jNy1KdPzEBwD5bVUsQJ4cmLAvsNP8-74Lm53tLtrF2I4d5k-pxsi59_bsHJLIqoaS5d1SqhQhZdx7A8NunfpOuP77nD9LLMkw1Nf1FTP8Xj7tkQeqmElOtHTuI_3Bm8WVywlAgPnxWV5dey7A7_3F117qP-0oDto7gA8KkLlGSA6jeY7WuyGpLVEba1WH6SzLrS9aZdi1FUtKHlh_SzMulkb-NmjDUP5dtv0GC6-QGdS8jkPeeXBT9dO5vD3lT-f8N9SF2dpgZVBp2zSpFHxjTt6SE1JC_wAH3MK7DeKO0Fh9cPVBPgR2zpRexM_vwkUtTLAjX69FY0zUSdgPeDSQ5D0BCuaUPmGAAn1OvSYwDTlBH21-wT1KC5yZKITgjbTu8_azjl-sSVCJegcjykLDnpsczb9ODx5UnhXzS4MG_-WWQdFDR65V8UUdoQP-XAZeneCus_-HyPg0TFpxmWIEKvCFH3iXhfaZnKzyKwggxWWRJpe3bwXGwORfOU1cut44l5rkzJL-duNTIdw80UaAdjuTEhOYnAXTWNneoGL8-tdGOEWb8AFRf5-wbHCYzhs9uZ37OoDiF1jUIfpsX7Xz7HWqIJIRVwW2CpHNWGCmmrxAdQIWHog3wPcZEhAxqppXoCylVCrEOaMX6CmHrM5WmsxwQiLpSJXhYwPiEIGzBa4B1WZfwFPYfMxejbWsjAXNM2BmyRatr_J3BoCfQLUkEF-wUkab3-PEc6xxUwy68tBpxp6qAkTKnRLhhzPdLYaQLdg6CJp0wddqsmDqlBAqoeXj5naYxz2u18txwZxjavOg5GNA0MXQiBfa_0LaVgUxLcDVy0gCCsnpHYSBqTROgII6JQCtGhC2b3K0jbTbd1Jph5I4eVZHbVdwZ5PmixvYLwc4ip-R1LQtiB2q70GurEWOpjzKAeZ5KeFISD0U_LOxD9CaQ0FRSLrjf8GuZp2389mtlSRd4_mJTCUhk6zKip3NOOS8F3PTuC2yycuot9HHH2AHmlH59D8jwu0pRmyed8emTk2t_GD2herAyZRvaJiu3RkPnY9_zUdhFyPiRIAFMWkkm2tMEZ3mJ5mycTKgNy79_bC5963lGmUQDeOeIBg5T9re5oYUQImdgfQbaRTnCoBU5dOaTYzG0Eu0JIPtcFEMAnWxWoYxkPD6xPyMKylByKGeae90HzQD3XhF7RmYfLAbKsQP0OfLHUvtzBXlPbDfzysT6Gx9ioRrtjP8UZ7UdGwJtOOuEeh2KLNKajahk1fnkFGdPjrQKOEhRHCqFCwl424B1y5t5R622Kl11RfAOjMWSZl5UIfwZUyPAE0SD2zbrnVxSHTkRgpiD7B6jMcLWyu2RoaG4hVr2xwcRBq3caQu2OJlxvXD19-2Z9qwI1ZlsFpWbltAJ0fq24vI4GOC7CeofPB0A43M-sO4tVwBZb-XVBx96Uw_eAnGsUJHEwmRzou3DodmmDVLZ8edVqVEGaL8_R-k-5t0pTQKmrl-U0PN1XdNT4fkQo9Edg-ElvMdQelJ08L4LEQ33HMOEn6Q_ltz38EYtRnQdipeiSSqMtXv_afx8ZxKyTTgVwWdHbGU8aKefmWeo0wqruHGqzL-ytIt0JnG87IzSh2uSb4ZnD8a49Btpe8BBm3ZNmW7pE3BexLSleAj7kTSuQ1iTTVLpxRfIdjbICagiQosePc3xFOKa54jraEId282gDQGWv2vUnaEFw7NrwaB1CR-MVRn5f7dvlWaVl3zqGVWuzaqDRDOHoBms8vlslpA_Ruxe9iswXwHWT0EYjbkEiiYxi2shIbH2cmA55vVlNTgM9ULCbsg2ZRV0KE6-0PcsLG4TNwLxidwHRTJbiW48zaMXK686dI8ULTrP1-Pa0VAVW61sMVuBrjc3SwxutppRmByRPKeUGDLwO6OZLIfSdW9tBg6vQAm6pxkiT3LUlFsgVF2r7gg9Kghb7tMVnjKHHQP22CTSrsuqXEyu9uCzpVMsrkSNqL7EXX-YBxmI2i8lJeDbvFFEm2Lq9AE1_uBs0hxROAObnso1RWBFrwgb-Fr53DBKb-aq2YhVv4Hwd5d3n9111sdlMPNuNZIowOiaBeb3sWbrdeoizc7iQcPqpFUEbFGPzlsf2WmumI68iCW18zFDLXBGM9YLr-9XC-xZCCvfaTUGM2k9qLnnDPEituFdDfTdEn374nQsauY0OSXUu0qALDtp8UHVHslAk37m1TApM1l6-kRLNJ8r5_X1V9vQAe1Pgwd7alhONT88z3Kj7m3BIsJKjHOkNmWpUi7iBc2d620MiMmdg4ttIY5qxGp33WSN7WFDMwaeP4K4Pe5eZ5hTvcEvpS7iTxdQQNCwhgs0F5tHDaITLXr2t54PyPFBgc8pRrKEHd3f1ZI3QqGhaoWKQtQrbpGB01CSh9ZTNMYDvUKmkO3rqHbuykfBxtF2hakyYGjfopv0UiNs9gKZOQz1uD1fbAi5FDbgsyOQxdlKKy53wNjERZn4UxPbrvYXmI_QIkw9AGHCvMFQWAYHNnGH0IGazRX25afny7RDzkdd81pc73f6dFCluJ6Kr8T_kcQB0ATypwIqHvM_q0nTW8fMVhHdf77k-2COdYCxZV3XOBM1HVt29nGbINdcjrkO5lcviw1prmrhzzHev0uB5rYwGrqRWU1VI6AnHKuDK89IZKB9szv8TqQLpuxtQPgp_ZcgzDKlYG8k24PbqR_z9vix6AEF5jCkkVg1xkK-dujCLkNkHEYZXtFLF3cxCYJBKLi_M2C5inItZu3tIVqADQZeK9L3iDYmC-g2KdnOsfuJnsZHRrglNKUa0n28l9cDOMMKczyr5ChInJ9B9NcgTRDNFfW01Kvrn6hffQ30tfBGwai83XLtG_7lGBRIxk8CYcZ9VMd94_l5JaordLWA6QN0mLi-RqeEaIf2zH5q5-YvVjki97yVu4qhYZVyQPvsarRonDrpu38T50-SNpfZU38suD0Qt7YGJaAxM-TDVWlY_QW26-_d7q8CGuZEU-IJn8EfqRNbFSK-p5sgHl4_Mw6W1HtTd4f9p4YZf6qWRwCLSSvLzfvNMJ82vlh96oVitMc1kIR8YbJpwJIS2KbQwhxHys-ocCh7oy76on1VNS9QCEhiztnHceBWMpKafjbBItl69W_ScJWUsk-ZyOts9IRH9HA5bGxK64mdb-pEqk4zu-moOGlh02em2IbDKsQ7XdHVSXypP7eudSiEQNPyFlUtNT7MjxrmESzs3PA96s6vzf4bX-3Jie2sxYr-eVK5evyYlSkLe6BsFbAZ2Rmn3th774hdfd4zjnJkZfhZrRpnuvxdzkwRM2vfkeYoaRm28KFWsAvtBHQ5cqT7stcTRDZXchls4MAFxT3OLSYIXloBX7aR5QSLtI4mZxMtRFqn3BrlRs5xicbh6Gxf44ocOi5S3IdkHig6lE6DqJIDTHhwE2YdW0d9Q6CYYYnyBSXNAzA21rs16DWs28QVlLM4DHIIWh3FUiiOGnpGLM0no4IXzFoClKFUgIPaInmW8NQVmHKpdzPfuGv_XfvHxz6Em-QYvvMXm4j4TsawIX64nwumhRwpaLnRt_gbmWPqCdCHX5HMXSQZCwaW01kqeE8PUYSC8rE989Pc824SdxKE5I48W72SRRMaeeGzcwlLnh1NACMXyHxr3nqKtEMqvRxPKFhvLgiWXpjN9gsnwmBHR0j52bm2zYv9IX1VgLYoypGcP2sl3MaZcn4RuFsSc6uNt--lsAJAJnfO2TaJvDkP-i9jclBmEaLeOBglH-ezsEFxYVEdvtp4PzQEc3NirCMKxErhHCe1Omn6DmFjWG49TmVWAiUxnlDhnO7WlaaCUZTtfX3JKBYwNpiMKFYE3i8ZQjelBMq_LWWtXj_950UgS-MNzmO2i_LX8C1SKWUyAcBpLxLOHMG875Sc8VKdX5NdyZ2Hju7Ps-95z_Bq0sf-lcVefSnbEbRP1rLoK14vCduYnJgtbz2Jddd_tKpgEyB1sMNh3X_0Uk_RdrFK7i5SIIl5GIOb_qDHttPP0Q__VOX9qwRcPZSgYeG7Nl5UT2v_tFRck4BtqPvTuUybTtEq7an14zvuu4JA4RSrAZmkHe0g9K1527oSa9k-bazoSR-htR5MLRat5Gm54Hi1q3y4TkMcSW-3KFwyc7Q_HYpPaG96xNlSj6eX8d-zqnbzs9koLFsdVGa_hZPXVvdK6Goig30epGyagpPJpkTiGCOCNvZ11_K3XWBzoDi7CjW1CAp2w65qERkDeCo1uDuOxbQcGtfrX7w7LmrcA4LPbIFXsOHBxdeVc4H-Xy_9FvAi3AQ-6wouHPvJ-262vc1RPytZ45F1ghnsJfs3FjAmQbdXkglavWG4wkigTyKrR02SstRzXbUle1WOR-i_NlXu7_MSALf1P4FJJIpOask60XBCG73K4Co_1W1H01mXKQGvBPuxon0M21h5ss9stlADcLj6yrEdOa_S-23UIq9JAh6BEO88oTqdJzITxH7oRKPFYQ6vfoJsBYpYCMEgTOQ9RRVfth_52dk0GKMC5JZ6Kf5lELUPiOtoP8n0xPIfxIKy58xd8tcxoIaHHyOvK2n68Dn88zvlUp-ZvB5Shnunf0ghVxw6E5DNkAPbRB90gVclrGsqvkDhzarxuoKZixvw-wt0LKCSDbV-_ONyDgoT6iz9G_LtmLEB2YrXpqOWAFatthrqvf0hvVcmm66RrLuOEP_yGFp8ICflMsJizeSpmK2D0GKxlWFP7rTlU8pXx3UcQEoCNClD2QW_Aquqvj-pUU3hTUcSOfjsnj8Xy-gl5nRj0YuFOE30T0oi-h94dZpmx3RgGV0XDeYoUjLPgp4atBbT0cKkVgY9in2rcyvr5Toarh4PKClhsutzcQLD2pz6p-nw9n0Ng13UnZ46E9tMQlLbEgfVtsUkrMnh6_y_4twZNfifQtLjGmFVpiN1nF6f93V9qlhuGOZPtRmBfVQQe3OH_SrMvS-UHKT_OQMPfyi7biozffauxxXFofw8nWliVihq64OI0AQYSFyaymtuHnDCMQLnOhhOtq1hZCFTAZFDdlpfRnVWfa0SEoIkVWHKsizoogI9cLYWcEkvf60KC2GYpcaQKfUNezfQmpJqSjUbcgqTfdlJVO4xLwM-qPaYMZul6bnZMIYXWg6RLYvNTm4wcPAP9OALofRplsZWbahvTAxvb3uGnRjMAlfg1WHNeXYomP4aEKqSxIdlC8ZuX-3hBU0uUEAOS3dWvfJ1Fzk3ZpzjPlAwzoyiNHiUdzhhHmx6XIkF7_cebhD63zG311YAJJoRDPfgV6I1FUXxmt7cSayxe-ffxhVOWsi2ZxFPo5hcUDoNebn5TcWGYgWGea3S9Dh4c3xBHxe8eKZijmyGEcvBOSrLldxspuMLX1RPbCtd4vWIZNLICik_Ezh6vtfPKxtlNTWbKef8fWczDi_pwC-v-9OHpAWh_WTDB3mxjAZiUnzhS4p-L22yZjQuIdZD3joF0ZVCQHlesTOaHeVGGGv0ydlXa4mgynH01uQMFd3j946QCjMNYgJdSXEAH_3lH9ctjPPGF3wNpO5Ee3f3BwmEBUa54VNPL66dLINjlZnCNtJlbLJ_aOQ-le0f97cZJsZl2tGD0h5mZbymwAE1Uk9Fm9ywEEEOi7RULZdPhGR30XWRz1t5jH4kOj0ZDhLnMkGMeM2p7PFPG-3mpwGYtNKsT92w12B-0_Jqx5NSHwKrMKB07WhKDC4zFH7T2Mx07W555EiVZE3PGx60fVNGVYEWpLLwL2N2W22RAwLdFLO2-aIyfSyNJ58oLeVXgBrDQadjP70bSV7wAz9mrY6gubRRFY65Uab9jxuAZn6a670D9VNu97YxvpyyVKQF8uHukBeQPUP9zH5F1kZ_IsTx5bs9lauatILNe3PWKvqQ5TRRqeLpDoH2Teno0QXcA1t3KNBGpst6r9x4zzGxr6dAHCZbtzhC1ufKNCfWHIqDDSnUgkdlHG-jaLA48HQo7xadRzzT5S9CSYUflDbVQku_zMDnIYaR9-Mc3mjDKpISvh4PxIKyH0m5GJi-whxqnh__NyqNfDe5MeokrZucw4oM6U3CDruwfKjBAo71iq2ivdBXnWnz8wKxcVK1v24R8yXNx5YAj7ep42C4tgRH1IshwWHHIEs1XKVaZuaVY6FRxDxTUjlU7xfAU_ELj1nVhmnn_wnQivpX7wDrQ7vcTyWXq18F6oed6Q7gXbAkL8R0bfqmQk4opwAR-Pip4TIpfUuigqnKj8nls7kVFCKdUxxuAZn6a670D9VNu97YxvpyyVKQF8uHukBeQPUP9zH5F1kZ_IsTx5bs9lauatILNe3PWKvqQ5TRRqeLpDoH2Teno0QXcA1t3KNBGpst6r9x4zzGxr6dAHCZbtzhC1ufKNCfWHIqDDSnUgkdlHG-jaLA48HQo7xadRzzT5S9CSYUflDbVQku_zMDnIYaR9-Mc3mjDKpISvh4PxIKyH0m5GJi-whxqnh__NyqNfDe5MeokrZucw4oM6U3CDruwfKjBAo71iq2ivdBXnWnz8wKxcVK1v24R8yXNx5YAj7ep42C4tgRH1IshwWHHIEs1XKVaZuaVY6FRxDxTUjlU7xfAU_ELj1nVhmnn_wnQivpX7wDrQ7vcTyWXq18F6oed6Q7gXbAkL8R0bfqmQk4opwAR-Pip4TIpfUuigqnKj8nls7kVFCKdUxh4-vk-WAvy6-gd0apY_1j6BsQF8X_2Jt6Q.W-FfmZTHbeZU0hCvgC8GTA',
            response: {
              timestamp: 1692705956788,       
              correlation_id: 'c7d0c6b1-5a86-4cd6-a72b-5867795a331c',
              api_call_id: 'b0eb2a81-75ad-4ef3-92f7-139361825085'
            }
          };

        const expectedIncoming = {
            headers: {
              alg: 'RSA-OAEP',
              enc: 'A256GCM',
              'x-hcx-api_call_id': 'e1c51ec8-c930-4c84-b55a-a244ff74fd1a',
              'x-hcx-timestamp': '2023-08-22T12:06:44.780Z',
              'x-hcx-workflow_id': '6c1df519-20c7-40d8-8c99-0794f9a7c6f9',
              'x-hcx-sender_code': 'testprovider1.swasthmock@swasth-hcx-staging',
              'x-hcx-recipient_code': 'testpayor1.swasthmock@swasth-hcx-staging',
              'x-hcx-correlation_id': 'e794efc5-4181-428e-84c4-ec221f4f1b09',
              kid: 'HiblymtUIyveU8p5R86Yw5l1UXv46IMFb0r2920CVtw'
            },
            payload: {
              resourceType: 'Bundle',
              id: '98aa81af-7a49-4159-a8ed-35e721d6ae74',
              meta: { lastUpdated: '2023-02-20T14:03:15.013+05:30', profile: [Array] },
              identifier: {
                system: 'https://www.tmh.in/bundle',
                value: '7ee7ee1a-fcad-49c3-8127-aa70c7a4dc0d'
              },
              type: 'collection',
              timestamp: '2023-02-20T14:03:15.013+05:30',
              entry: [ [Object], [Object], [Object], [Object], [Object] ]
            }
          };

        return responseIncoming && responseOutgoing;

    } catch (error) {
        console.error("An error occurred while processing operations:", error);
        return false;
    }
}

const config = {
    participantCode: "testprovider1.swasthmock@swasth-hcx-staging",
    authBasePath:
      "http://staging-hcx.swasth.app/api/v0.8/participant/auth/token/generate",
    protocolBasePath: "https://staging-hcx.swasth.app/api/v0.8",
    encryptionPrivateKeyURL:
      "https://raw.githubusercontent.com/Swasth-Digital-Health-Foundation/hcx-platform/main/hcx-apis/src/test/resources/examples/x509-private-key.pem",
    username: "testprovider1@swasthmock.com",
    password: "Opensaber@123",
    igUrl: "https://ig.hcxprotocol.io/v0.7.1",
  };
  
  const fhirPayload = {
    resourceType: "Bundle",
    id: "98aa81af-7a49-4159-a8ed-35e721d6ae74",
    meta: {
      lastUpdated: "2023-02-20T14:03:15.013+05:30",
      profile: [
        "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-ClaimRequestBundle.html",
      ],
    },
    identifier: {
      system: "https://www.tmh.in/bundle",
      value: "7ee7ee1a-fcad-49c3-8127-aa70c7a4dc0d",
    },
    type: "collection",
    timestamp: "2023-02-20T14:03:15.013+05:30",
    entry: [
      {
        fullUrl: "Claim/bb1eea08-8739-4f14-b541-04622f18450c",
        resource: {
          resourceType: "Claim",
          id: "bb1eea08-8739-4f14-b541-04622f18450c",
          meta: {
            lastUpdated: "2023-02-20T14:03:14.918+05:30",
            profile: [
              "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Claim.html",
            ],
          },
          identifier: [
            {
              system: "http://identifiersystem.com",
              value: "IdentifierValue",
            },
          ],
          status: "active",
          type: {
            coding: [
              {
                system: "http://terminology.hl7.org/CodeSystem/claim-type",
                code: "institutional",
              },
            ],
          },
          use: "claim",
          patient: {
            reference: "Patient/RVH1003",
          },
          created: "2023-02-20T14:03:14+05:30",
          insurer: {
            reference: "Organization/GICOFINDIA",
          },
          provider: {
            reference: "Organization/WeMeanWell01",
          },
          priority: {
            coding: [
              {
                system: "http://terminology.hl7.org/CodeSystem/processpriority",
                code: "normal",
              },
            ],
          },
          payee: {
            type: {
              coding: [
                {
                  system: "http://terminology.hl7.org/CodeSystem/payeetype",
                  code: "provider",
                },
              ],
            },
            party: {
              reference: "Organization/WeMeanWell01",
            },
          },
          careTeam: [
            {
              sequence: 4,
              provider: {
                reference: "Organization/WeMeanWell01",
              },
            },
          ],
          diagnosis: [
            {
              sequence: 1,
              diagnosisCodeableConcept: {
                coding: [
                  {
                    system: "http://irdai.com",
                    code: "E906184",
                    display: "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY",
                  },
                ],
                text: "SINGLE INCISION LAPAROSCOPIC APPENDECTOMY",
              },
              type: [
                {
                  coding: [
                    {
                      system:
                        "http://terminology.hl7.org/CodeSystem/ex-diagnosistype",
                      code: "admitting",
                      display: "Admitting Diagnosis",
                    },
                  ],
                },
              ],
            },
          ],
          insurance: [
            {
              sequence: 1,
              focal: true,
              coverage: {
                reference: "Coverage/COVERAGE1",
              },
            },
          ],
          item: [
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E101021",
                    display: "Twin Sharing Ac",
                  },
                ],
              },
              unitPrice: {
                value: 100000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E924260",
                    display: "CLINICAL TOXICOLOGY SCREEN, BLOOD",
                  },
                ],
              },
              unitPrice: {
                value: 2000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E924261",
                    display: "CLINICAL TOXICOLOGY SCREEN,URINE",
                  },
                ],
              },
              unitPrice: {
                value: 1000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E507029",
                    display: "ECG",
                  },
                ],
              },
              unitPrice: {
                value: 5000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E6080377",
                    display: "UltraSound Abdomen",
                  },
                ],
              },
              unitPrice: {
                value: 5000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "502001",
                    display: "Surgeons Charges",
                  },
                ],
              },
              unitPrice: {
                value: 1000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "5020021",
                    display: "Anesthesiologist charges",
                  },
                ],
              },
              unitPrice: {
                value: 1000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E6080373",
                    display: "Physician charges",
                  },
                ],
              },
              unitPrice: {
                value: 1000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "201008",
                    display: "Recovery Room",
                  },
                ],
              },
              unitPrice: {
                value: 10000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "406003",
                    display: "intra -venous (iv) set",
                  },
                ],
              },
              unitPrice: {
                value: 5000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E507353",
                    display: "Oral Medication",
                  },
                ],
              },
              unitPrice: {
                value: 5000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "E925171",
                    display: "Hospital charges",
                  },
                ],
              },
              unitPrice: {
                value: 5000,
                currency: "INR",
              },
            },
            {
              sequence: 1,
              productOrService: {
                coding: [
                  {
                    system: "https://irdai.gov.in/package-code",
                    code: "501001",
                    display: "Consultation Charges",
                  },
                ],
              },
              unitPrice: {
                value: 5000,
                currency: "INR",
              },
            },
          ],
          total: {
            value: 146000.0,
            currency: "INR",
          },
        },
      },
      {
        fullUrl: "Organization/WeMeanWell01",
        resource: {
          resourceType: "Organization",
          id: "WeMeanWell01",
          meta: {
            profile: [
              "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization",
            ],
          },
          identifier: [
            {
              type: {
                coding: [
                  {
                    system: "http://terminology.hl7.org/CodeSystem/v2-0203",
                    code: "AC",
                    display: "Narayana",
                  },
                ],
              },
              system: "http://abdm.gov.in/facilities",
              value: "HFR-ID-FOR-TMH",
            },
          ],
          name: "WeMeanWell Hospital",
          address: [
            {
              text: " Bannerghatta Road, Bengaluru ",
              city: "Bengaluru",
              country: "India",
            },
          ],
        },
      },
      {
        fullUrl: "Organization/GICOFINDIA",
        resource: {
          resourceType: "Organization",
          id: "GICOFINDIA",
          meta: {
            profile: [
              "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Organization",
            ],
          },
          identifier: [
            {
              type: {
                coding: [
                  {
                    system: "http://terminology.hl7.org/CodeSystem/v2-0203",
                    code: "AC",
                    display: "GOVOFINDIA",
                  },
                ],
              },
              system: "http://irdai.gov.in/insurers",
              value: "GICOFINDIA",
            },
          ],
          name: "GICOFINDIA",
        },
      },
      {
        fullUrl: "Patient/RVH1003",
        resource: {
          resourceType: "Patient",
          id: "RVH1003",
          meta: {
            profile: [
              "https://nrces.in/ndhm/fhir/r4/StructureDefinition/Patient",
            ],
          },
          identifier: [
            {
              type: {
                coding: [
                  {
                    system: "http://terminology.hl7.org/CodeSystem/v2-0203",
                    code: "SN",
                    display: "Subscriber Number",
                  },
                ],
              },
              system: "http://gicofIndia.com/beneficiaries",
              value: "BEN-101",
            },
          ],
          name: [
            {
              text: "Prasidh Dixit",
            },
          ],
          gender: "male",
          birthDate: "1960-09-26",
          address: [
            {
              text: "#39 Kalena Agrahara, Kamanahalli, Bengaluru - 560056",
              city: "Bengaluru",
              state: "Karnataka",
              postalCode: "560056",
              country: "India",
            },
          ],
        },
      },
      {
        fullUrl: "Coverage/COVERAGE1",
        resource: {
          resourceType: "Coverage",
          id: "COVERAGE1",
          meta: {
            profile: [
              "https://ig.hcxprotocol.io/v0.7.1/StructureDefinition-Coverage.html",
            ],
          },
          identifier: [
            {
              system: "https://www.gicofIndia.in/policies",
              value: "policy-RVH1003",
            },
          ],
          status: "active",
          subscriber: {
            reference: "Patient/RVH1003",
          },
          subscriberId: "2XX8971",
          beneficiary: {
            reference: "Patient/RVH1003",
          },
          relationship: {
            coding: [
              {
                system:
                  "http://terminology.hl7.org/CodeSystem/subscriber-relationship",
                code: "self",
              },
            ],
          },
          payor: [
            {
              reference: "Organization/GICOFINDIA",
            },
          ],
        },
      },
    ],
  };

validateHCXIntegratorOperations(config, fhirPayload).then(result => {
    if (result) {
        console.log("The operation responses match the expected outcomes!");
    } else {
        console.log("The operation responses did not match the expected outcomes.");
    }
});
