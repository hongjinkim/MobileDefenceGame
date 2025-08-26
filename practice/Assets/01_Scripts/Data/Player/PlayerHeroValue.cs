using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerHeroState
{
    public string HeroId; // ���� ID
    public int Level; // ���� ����
    public int Star; // ���� �� ���
    public BigNum Exp; // ����ġ

    public Dictionary<EEquipmentType, EquipmentValue> Equipped = new()
    {
        { EEquipmentType.Weapon,    null },
        { EEquipmentType.Helmet,     null },
        { EEquipmentType.Armor,null },
        { EEquipmentType.Necklace,null },
        { EEquipmentType.Ring,   null },
        { EEquipmentType.Shoes,  null }
    };

    public PlayerHeroState() { }
    public PlayerHeroState(string heroId, int level = 1, int star = 1)
    {
        HeroId = heroId;
        Level = level;
        Star = star;
        Exp = 0;
    }
}

// UI/��Ʈ��ũ ���ۿ� ��(������ �̸�/�Ӽ��� �ٿ��� ����)
[Serializable]
public class PlayerHeroView
{
    public string HeroId;
    public string Name;
    public EGrade Grade;
    public string Element;

    public int Level;
    public int Star;
    public BigNum Exp;
}

[Serializable]
public class PlayerHeroValue : BaseSaveData<PlayerHeroValue>
{
    // ���� ����: heroId -> ��������
    [ShowInInspector]
    public Dictionary<string, PlayerHeroState> Owned = new();

    // ���� ����: heroId -> Hidden/Seen/Owned
    [ShowInInspector]
    public Dictionary<string, EHeroDiscoveryState> Collection = new();

    // ��(��): deckId -> heroId ����Ʈ
    [ShowInInspector]
    public Dictionary<string, List<string>> Decks = new(); // ��: "Team1"  ��

    // ���� �⺻ ���� ������������������������������������������������������������������������������������������
    public bool IsOwned(string heroId) => Owned.ContainsKey(heroId);

    public PlayerHeroState Acquire(string heroId, int level = 1, int star = 1)
    {
        if (!Owned.TryGetValue(heroId, out var s))
        {
            s = new PlayerHeroState(heroId, level, star);
            Owned[heroId] = s;
        }
        PromoteCollection(heroId, EHeroDiscoveryState.Owned);
        return s;
    }

    public void Seen(string heroId) => PromoteCollection(heroId, EHeroDiscoveryState.Seen);

    private void PromoteCollection(string heroId, EHeroDiscoveryState target)
    {
        if (!Collection.TryGetValue(heroId, out var cur) || (int)target > (int)cur)
            Collection[heroId] = target;
    }

    /// �����Ϳ� �����ϴ� ��� heroId�� ���� Collection Ű�� ����(Hidden���� ä��)
    public void EnsureCollectionKeys(HeroData master)
    {
        foreach (var id in master.HeroDict.Keys)
            if (!Collection.ContainsKey(id))
                Collection[id] = EHeroDiscoveryState.Hidden;
    }



    // ���� �� ����(������ �̸�/�Ӽ� ���̱�) ������������������������������������������
    public IEnumerable<PlayerHeroView> GetCollectionList(HeroData master,
        Func<HeroValue, bool> filter = null)
    {
        // ������ ��ü ������ ����
        foreach (var kv in master.HeroDict)
        {
            var m = kv.Value;
            if (filter != null && !filter(m)) continue;

            Owned.TryGetValue(kv.Key, out var owned);
            // ���� ���°� ���� ���� ������ Hidden���� �⺻ ����
            Collection.TryGetValue(kv.Key, out var state);

            // Hidden�̾ ��� �̵�, UI���� ���·� ����/����ŷ ����
            yield return new PlayerHeroView
            {
                HeroId = kv.Key,
                Name = m.Name,
                Grade = m.Grade,

                Level = owned?.Level ?? 0,
                Star = owned?.Star ?? 0,
                Exp = owned?.Exp ?? 0,
            };
        }
    }

    public IEnumerable<PlayerHeroView> GetOwnedList(HeroData master)
    {
        foreach (var kv in Owned)
        {
            if (!master.HeroDict.TryGetValue(kv.Key, out var m)) continue; // �����Ϳ� ���� ID ���
            var s = kv.Value;
            yield return new PlayerHeroView
            {
                HeroId = kv.Key,
                Name = m.Name,
                Grade = m.Grade,

                Level = s.Level,
                Star = s.Star,
                Exp = s.Exp
            };
        }
    }

    // ���� �� ���� ��������������������������������������������������������������������������������������������
    public IReadOnlyList<string> GetDeck(string deckId)
        => Decks.TryGetValue(deckId, out var list) ? list : Array.Empty<string>();

    /// �� ���� ����
    /// ownedOnly=true�� �������� ���� ������ ����
    public bool SetDeck(string deckId, IEnumerable<string> heroIds, int maxSize = 5, bool ownedOnly = true)
    {
        if (heroIds == null) return false;

        // �ߺ� ���� + ���� ����
        var distinct = new List<string>();
        var seen = new HashSet<string>();
        foreach (var id in heroIds)
        {
            if (string.IsNullOrEmpty(id)) continue;
            if (!seen.Add(id)) continue;
            if (ownedOnly && !IsOwned(id)) continue;
            distinct.Add(id);
            if (distinct.Count >= maxSize) break;
        }

        Decks[deckId] = distinct;
        return true;
    }

    public IEnumerable<PlayerHeroView> GetDeckViews(string deckId, HeroData master)
    {
        if (!Decks.TryGetValue(deckId, out var ids)) yield break;

        foreach (var id in ids)
        {
            if (!master.HeroDict.TryGetValue(id, out var m)) continue;
            Owned.TryGetValue(id, out var s);
            yield return new PlayerHeroView
            {
                HeroId = id,
                Name = m.Name,
                Grade = m.Grade,

                Level = s?.Level ?? 0,
                Star = s?.Star ?? 0,
                Exp = s?.Exp ?? 0
            };
        }
    }

    // ������ �ִ� ��� ������ Seen���� ǥ��
    public void SetAllSeen(HeroData master)
    {
        
        foreach (var id in master.HeroDict.Keys)
        {
            if (Collection.TryGetValue(id, out var state) && state < EHeroDiscoveryState.Seen)
            {
                Collection[id] = EHeroDiscoveryState.Seen;
            }
        }
    }
}
