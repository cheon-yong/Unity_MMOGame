using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

        GetButton((int)Buttons.Confirm).gameObject.BindEvent(OnClickButton);
        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickButton);
    }

    public void SetError(Define.Error error)
    {
        switch (error)
        {
            case Define.Error.CreateAccount:
                SetText("���� ���� ����",
                        "���� ������ �����Ͽ����ϴ�",
                        "Ȯ��");
                break;
            case Define.Error.Login:
                SetText("�α��� ����",
                        "�α��ο� �����Ͽ����ϴ�",
                        "Ȯ��");
                break;
        }
    }

    public void SetNotice(Define.Notice notice)
    {
        switch (notice)
        {
            case Define.Notice.CreateAccount:
                SetText("���� ���� ����", 
                    "���� ������ �����Ͽ����ϴ�.", 
                    "Ȯ��");
                break;
        }
    }

    public void SetText(string title, string error, string button)
    {
        GetText((int)Texts.TitleText).text = title;
        GetText((int)Texts.ErrorText).text = error;
        GetText((int)Texts.ButtonText).text = button;
    }

    void OnClickButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI();
    }
}
