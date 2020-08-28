using UnityEngine;
using System.Collections;
using System;

public class RocketWallet 
{
    #region Eventos
    public event Action<AchievementType> onCheckAchievement;
    public event Action<int> onFragmentsAmountChanged;

    #endregion

    const int YELLOW = 0;
    const int MAGENTA = 1;
    const int RED = 2;
    const int WHITE = 3;

    private int _fragments;
    public int fragments
    { get { return _fragments; } }

    private int _lastFragTaken;
    public int lastFragmentTaken
    { get { return _lastFragTaken; } }

    private int[] _totalFragByKind;
    public int[] totalFragByKind
    { get { return _totalFragByKind; } }

    public int totalYellowFrag
    { get { return _totalFragByKind[YELLOW]; } }

    public int totalMagentaFrag
    { get { return _totalFragByKind[MAGENTA]; } }

    public int totalRedFrag
    { get { return _totalFragByKind[RED]; } }

    public int totalWhiteFrag
    { get { return _totalFragByKind[WHITE]; } }

    public RocketWallet()
    {
        _fragments = 0;
        _lastFragTaken = 0;
        _totalFragByKind = new int[] { 0, 0, 0, 0 };
   
    }

    public void AddFragment(int amount)
    {
        _fragments += amount;
        _lastFragTaken = amount;
        switch (amount)
        {
            case 1: _totalFragByKind[YELLOW]++; break;
            case 3: _totalFragByKind[MAGENTA]++; break;
            case 5: _totalFragByKind[RED]++; break;
            case 10: _totalFragByKind[WHITE]++; break;
        }

        // sinal para atualizar interface
        // fragsLabel.text = fragments.ToString();
        if (onFragmentsAmountChanged != null)
            onFragmentsAmountChanged(fragments);

        // persistindo os fragmentos
        int frags = ShipStatusData.GetTotalFragments();
        ShipStatusData.SetTotalFragments(frags + amount);

        // checa Achievement de FRAG
        if (onCheckAchievement != null)
            onCheckAchievement(AchievementType.Fragment);
    }   
}
