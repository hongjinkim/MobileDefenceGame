using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerHeroState
{
    public string HeroId; // 영웅 ID
    public int Level; // 영웅 레벨
    public int Star; // 영웅 별 등급
    public BigNum Exp; // 경험치

    public PlayerHeroState() { }
    public PlayerHeroState(string heroId)
    {
        HeroId = heroId;
        Level = 1;
        Star = 1;
        Exp = 0;
    }
}

// UI/네트워크 전송용 뷰(마스터 이름/속성을 붙여서 제공)
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
    // 보유 상태: heroId -> 성장정보
    public Dictionary<string, PlayerHeroState> Owned = new();

    // 도감 상태: heroId -> Hidden/Seen/Owned
    public Dictionary<string, EHeroDiscoveryState> Codex = new();

    // 덱(팀): deckId -> heroId 리스트
    public Dictionary<string, List<string>> Decks = new(); // 예: "Team1"  등

    // ── 기본 쿼리 ─────────────────────────────────────────────
    public bool IsOwned(string heroId) => Owned.ContainsKey(heroId);

    public PlayerHeroState Acquire(string heroId)
    {
        if (!Owned.TryGetValue(heroId, out var s))
        {
            s = new PlayerHeroState(heroId);
            Owned[heroId] = s;
        }
        PromoteCodex(heroId, EHeroDiscoveryState.Owned);
        return s;
    }

    public void Seen(string heroId) => PromoteCodex(heroId, EHeroDiscoveryState.Seen);

    private void PromoteCodex(string heroId, EHeroDiscoveryState target)
    {
        if (!Codex.TryGetValue(heroId, out var cur) || (int)target > (int)cur)
            Codex[heroId] = target;
    }

    /// 마스터에 존재하는 모든 heroId에 대해 Codex 키를 보장(Hidden으로 채움)
    public void EnsureCodexKeys(HeroData master)
    {
        foreach (var id in master.HeroDict.Keys)
            if (!Codex.ContainsKey(id))
                Codex[id] = EHeroDiscoveryState.Hidden;
    }

    // ── 뷰 생성(마스터 이름/속성 붙이기) ─────────────────────
    public IEnumerable<PlayerHeroView> GetCodexList(HeroData master,
        Func<HeroValue, bool> filter = null)
    {
        // 도감은 전체 마스터 기준
        foreach (var kv in master.HeroDict)
        {
            var m = kv.Value;
            if (filter != null && !filter(m)) continue;

            Owned.TryGetValue(kv.Key, out var owned);
            // 도감 상태가 없을 수도 있으니 Hidden으로 기본 제공
            Codex.TryGetValue(kv.Key, out var state);

            // Hidden이어도 뷰는 뽑되, UI에서 상태로 필터/마스킹 가능
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
            if (!master.HeroDict.TryGetValue(kv.Key, out var m)) continue; // 마스터에 없는 ID 방어
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

    // ── 덱 관리 ──────────────────────────────────────────────
    public IReadOnlyList<string> GetDeck(string deckId)
        => Decks.TryGetValue(deckId, out var list) ? list : Array.Empty<string>();

    /// 덱 구성 세팅
    /// ownedOnly=true면 보유하지 않은 영웅은 제외
    public bool SetDeck(string deckId, IEnumerable<string> heroIds, int maxSize = 5, bool ownedOnly = true)
    {
        if (heroIds == null) return false;

        // 중복 제거 + 순서 유지
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
}
