﻿using UnityEngine;
using GameFramework.Event;

public partial class MenuUIForm : UIFormBase
{
    [SerializeField] bool showLvSwitch = false;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        GF.Event.Subscribe(UserDataChangedEventArgs.EventId, OnUserDataChanged);
        GF.Event.Subscribe(PlayerEventArgs.EventId, OnPlayerEvent);
        SetLevelNumText(GF.DataModel.GetOrCreate<PlayerDataModel>().GAME_LEVEL);
        RefreshMoneyText();
    }
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        GF.Event.Unsubscribe(UserDataChangedEventArgs.EventId, OnUserDataChanged);
        GF.Event.Unsubscribe(PlayerEventArgs.EventId, OnPlayerEvent);
        base.OnClose(isShutdown, userData);
    }

    private void OnPlayerEvent(object sender, GameEventArgs e)
    {
        var args = e as PlayerEventArgs;
        
    }

    protected override void OnButtonClick(object sender, string btId)
    {
        base.OnButtonClick(sender, btId);
        switch (btId)
        {
            case "SHOP":
                GF.UI.OpenUIForm(UIViews.ShopUIForm);
                break;
            case "SETTING":
                GF.UI.OpenUIForm(UIViews.SettingDialog);
                break;
        }
    }

    public void SetLevelProgress(float progress)
    {
        levelProgress.value = progress;
    }
    public float GetLevelProgress()
    {
        return levelProgress.value;
    }
    private void OnUserDataChanged(object sender, GameEventArgs e)
    {
        var args = e as UserDataChangedEventArgs;
        switch (args.Type)
        {
            case UserDataType.MONEY:
                RefreshMoneyText();
                break;
            case UserDataType.GAME_LEVEL:
                SetLevelNumText((int)args.Value);
                break;
        }
    }
    internal void SetLevelNumText(int id)
    {
        levelText.text = id.ToString();
        var lvTb = GF.DataTable.GetDataTable<LevelTable>();
        int nextLvId = Const.RepeatLevel ? id + 1 : Mathf.Min(id + 1, lvTb.MaxIdDataRow.Id);
        nextLevelText.text = nextLvId.ToString();
    }

    private void RefreshMoneyText()
    {
        var playerDm = GF.DataModel.GetOrCreate<PlayerDataModel>();
        SetMoneyText(playerDm.Coins);
    }
    private void SetMoneyText(int money)
    {
        moneyText.text = UtilityBuiltin.Valuer.ToCoins(money);
    }
    public void SwitchLevel(int dir)
    {
        GF.DataModel.GetOrCreate<PlayerDataModel>().GAME_LEVEL += dir;
        var menuProcedure = GF.Procedure.CurrentProcedure as MenuProcedure;
        if (null != menuProcedure)
        {
            menuProcedure.ShowLevel();
        }
    }
}
