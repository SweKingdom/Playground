using System.Collections.Immutable;
using PlayGround.Extensions;
using PlayGround.Lesson04.CoinFlip.Models;

namespace PlayGround.Lesson04.CoinFlip;

public static class CoinFlipGame
{
    public static void RunSimulation()
    {

        System.Console.WriteLine("\nCoin Flip Round Simulation:");
        System.Console.WriteLine("Your code should implement the Coin Flip round exercises below.");
        System.Console.WriteLine("========================");

        // Implement a Coin Flip round simulation here using functional patterns
        // following the exercise instructions in the CoinFlipExercises.md file.

        // Exc 1-2
        var coin = new Coin(CoinSide.Heads);

        var coin1 = coin.Flip();
        
        var tCoin = new Coin(CoinSide.Tails);
        
        var tGuess = tCoin.ValidateGuess(CoinSide.Heads);
        
        var tGuess1 = tCoin.Flip().ValidateGuess(CoinSide.Tails);
        
        
        string result = tGuess switch
        {
            true => "Correct guess",
            false => "Wrong guess"

        };
        Console.WriteLine(result);
        
        string result1 = tGuess1 switch
        {
            true => "Correct guess",
            false => "Wrong guess"

        };
        
        //Exc 3
        var player1 = new Player("Alice", new Coin(CoinSide.Heads));
        var player2 = new Player("Bob", new Coin(CoinSide.Tails));

        // Display original players
        Console.WriteLine("Original players:");
        Console.WriteLine(player1);
        Console.WriteLine(player2);

        Console.WriteLine();

        // Flip coin
        var updatedPlayer1 = player1 with
        {
            Coin = player1.Coin.Flip()
        };

        // Display updated player and original player
        Console.WriteLine("After flipping Alice's coin:");
        Console.WriteLine($"Original Alice: {player1}");
        Console.WriteLine($"Updated Alice:  {updatedPlayer1}");

        Console.WriteLine(result1);
        
        

    }
}