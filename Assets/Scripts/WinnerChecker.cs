using System;
using System.Collections.Generic;
using UnityEngine;

public readonly struct Case
{
    readonly public Vector3Int firstPosition;
    readonly public Vector3Int thirdPosition;

    public Case(Vector3Int firstPosition, Vector3Int thirdPosition)
    {
        this.firstPosition = firstPosition;
        this.thirdPosition = thirdPosition;
    }

    public override bool Equals(object obj)
    {
        if (obj is Case)
        {
            Case @case = (Case)obj;

            return (
                (@case.firstPosition == this.firstPosition && @case.thirdPosition == this.thirdPosition) ||
                (@case.firstPosition == this.thirdPosition && @case.thirdPosition == this.firstPosition)
            );
        }
        return false;
    }
    public override int GetHashCode()
    {
        return firstPosition.GetHashCode() ^ thirdPosition.GetHashCode();
    }
    static public bool operator ==(Case a, Case b)
    {
        return a.Equals(b);
    }
    static public bool operator !=(Case a, Case b)
    {
        return !a.Equals(b);
    }

    public bool IsCorrect()
    {
        int x1 = firstPosition.x;
        int y1 = firstPosition.y;
        int z1 = firstPosition.z;

        int x3 = thirdPosition.x;
        int y3 = thirdPosition.y;
        int z3 = thirdPosition.z;

        return Mathf.Abs(x1 - x3) != 1 &&
            Mathf.Abs(y1 - y3) != 1 &&
            Mathf.Abs(z1 - z3) != 1;
    }
}



public class WinnerChecker : MonoBehaviour
{
    Case[] patterns = {
        new(new(0, 0, 0), new(0, 0, 2)),
        new(new(0, 0, 0), new(0, 2, 0)),
        new(new(0, 0, 0), new(0, 2, 2)),
        new(new(0, 0, 0), new(2, 0, 0)),
        new(new(0, 0, 0), new(2, 0, 2)),
        new(new(0, 0, 0), new(2, 2, 0)),
        new(new(0, 0, 0), new(2, 2, 2)),
        new(new(0, 0, 1), new(0, 2, 1)),
        new(new(0, 0, 1), new(2, 0, 1)),
        new(new(0, 0, 1), new(2, 2, 1)),
        new(new(0, 0, 2), new(0, 2, 0)),
        new(new(0, 0, 2), new(0, 2, 2)),
        new(new(0, 0, 2), new(2, 0, 0)),
        new(new(0, 0, 2), new(2, 0, 2)),
        new(new(0, 0, 2), new(2, 2, 0)),
        new(new(0, 0, 2), new(2, 2, 2)),
        new(new(0, 1, 0), new(0, 1, 2)),
        new(new(0, 1, 0), new(2, 1, 0)),
        new(new(0, 1, 0), new(2, 1, 2)),
        new(new(0, 1, 1), new(2, 1, 1)),
        new(new(0, 1, 2), new(2, 1, 0)),
        new(new(0, 1, 2), new(2, 1, 2)),
        new(new(0, 2, 0), new(0, 2, 2)),
        new(new(0, 2, 0), new(2, 0, 0)),
        new(new(0, 2, 0), new(2, 0, 2)),
        new(new(0, 2, 0), new(2, 2, 0)),
        new(new(0, 2, 0), new(2, 2, 2)),
        new(new(0, 2, 1), new(2, 0, 1)),
        new(new(0, 2, 1), new(2, 2, 1)),
        new(new(0, 2, 2), new(2, 0, 0)),
        new(new(0, 2, 2), new(2, 0, 2)),
        new(new(0, 2, 2), new(2, 2, 0)),
        new(new(0, 2, 2), new(2, 2, 2)),
        new(new(1, 0, 0), new(1, 0, 2)),
        new(new(1, 0, 0), new(1, 2, 0)),
        new(new(1, 0, 0), new(1, 2, 2)),
        new(new(1, 0, 1), new(1, 2, 1)),
        new(new(1, 0, 2), new(1, 2, 0)),
        new(new(1, 0, 2), new(1, 2, 2)),
        new(new(1, 1, 0), new(1, 1, 2)),
        new(new(1, 2, 0), new(1, 2, 2)),
        new(new(2, 0, 0), new(2, 0, 2)),
        new(new(2, 0, 0), new(2, 2, 0)),
        new(new(2, 0, 0), new(2, 2, 2)),
        new(new(2, 0, 1), new(2, 2, 1)),
        new(new(2, 0, 2), new(2, 2, 0)),
        new(new(2, 0, 2), new(2, 2, 2)),
        new(new(2, 1, 0), new(2, 1, 2)),
        new(new(2, 2, 0), new(2, 2, 2))
    };
    GameManager gameManager;

    public Dictionary<Player, Case[]> results = new();

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public Player GetWinner()
    {
        Player caseWinningPlayer = null;

        List<Case> winningCases = new();

        foreach (var pretendent in patterns)
        {
            if (WhetherCaseWasHandled(pretendent)) continue;

            var temporaryCaseWinningPlayer = GetCaseWinningPlayer(pretendent);

            if (temporaryCaseWinningPlayer != null)
            {
                caseWinningPlayer = temporaryCaseWinningPlayer;
                winningCases.Add(pretendent);
            }
        }

        if (caseWinningPlayer != null)
        {
            results.Add(caseWinningPlayer, winningCases.ToArray());
        }

        return caseWinningPlayer;
    }

    bool WhetherCaseWasHandled(Case caseToCheck)
    {
        foreach (var cases in results.Values)
        {
            foreach (var @case in cases)
            {
                if (caseToCheck == @case) return true;
            }
        }

        return false;
    }

    Player GetCaseWinningPlayer(Case pretendent)
    {
        if (!pretendent.IsCorrect()) return null;

        var firstPosition = pretendent.firstPosition;
        var thirdPosition = pretendent.thirdPosition;

        int x1 = firstPosition.x;
        int y1 = firstPosition.y;
        int z1 = firstPosition.z;

        int x3 = thirdPosition.x;
        int y3 = thirdPosition.y;
        int z3 = thirdPosition.z;

        int x2 = (x1 + x3) / 2;
        int y2 = (y1 + y3) / 2;
        int z2 = (z1 + z3) / 2;

        GameObject firstGO  = gameManager.gameCharacters[x1, y1, z1];
        GameObject secondGO = gameManager.gameCharacters[x2, y2, z2];
        GameObject thirdGO  = gameManager.gameCharacters[x3, y3, z3];

        if (firstGO.CompareTag("Unoccupied")) return null;

        if (firstGO.CompareTag(secondGO.tag) && secondGO.CompareTag(thirdGO.tag))
        {
            return Array.Find(
                gameManager.players, 
                player => firstGO.CompareTag(player.gameObjectTag)
            );
        }

        return null;
    }
}
