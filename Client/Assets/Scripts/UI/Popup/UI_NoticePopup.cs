using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NoticePopup : UI_Popup
{
    enum Buttons
    {
        Background,
        Confirm,
    }

    enum Texts
    {
        TitleText,
        ErrorText,
        ButtonText,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
    }

    public void SetError(Define.Error error)
    {
        switch (error)
        {
            case Define.Error.CreateAccount:
                SetText("계정 생성 실패",
                        "계정 생성에 실패하였습니다",
                        "확인");
                break;
            case Define.Error.Login:
                SetText("로그인 실패",
                        "로그인에 실패하였습니다",
                        "확인");
                break;
        }
    }

    public void SetNotice(Define.Notice notice)
    {
        switch (notice)
        {
            case Define.Notice.CreateAccount:
                SetText("계정 생성 성공", 
                    "계정 생성에 성공하였습니다.", 
                    "확인");
                break;
        }
    }

    public void SetText(string title, string error, string button)
    {
        GetText((int)Texts.TitleText).text = title;
        GetText((int)Texts.ErrorText).text = error;
        GetText((int)Texts.ButtonText).text = button;
    }
}
