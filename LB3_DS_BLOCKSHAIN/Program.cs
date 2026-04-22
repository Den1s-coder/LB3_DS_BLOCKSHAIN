using System;
using System.Collections.Generic;
using LB3_DS_BLOCKSHAIN;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var network = new Blockchain();
        Console.WriteLine("МЕРЕЖА ІНІЦІАЛІЗОВАНА");


        var user = Account.GenAccount();
        var userId = user.GetAccountID();

        network.RegisterUser(user, "Denis", "Розробник");
        var profile = network.GetUserProfile(userId);

        profile.PrintProfile();

        var post1 = new Post(user, "Тест пост 1");
        var post2 = new Post(user, "Тест пост 2");
        var post3 = new Post(user, "Тест пост 3");
        var post4 = new Post(user, "Тест пост 4");

        network.CreatePost(post1);
        network.CreatePost(post2);
        network.CreatePost(post3);
        network.CreatePost(post4);

        Console.WriteLine($"Створено 4 пости\n");
        post1.Print();
        post2.Print();
        post3.Print();
        post4.Print();

        var comment1 = new Comment(user, post1.GetPostID(), "Тест комент 1");
        var comment2 = new Comment(user, post1.GetPostID(), "Тест комент 2");
        var comment3 = new Comment(user, post2.GetPostID(), "Тест комент 3");
        var comment4 = new Comment(user, post3.GetPostID(), "Тест комент 4");
        var comment5 = new Comment(user, post4.GetPostID(), "Тест комент 5");

        network.AddComment(comment1);
        network.AddComment(comment2);
        network.AddComment(comment3);
        network.AddComment(comment4);
        network.AddComment(comment5);

        Console.WriteLine($"Додано 5 коментарів\n");
        comment1.Print();
        comment2.Print();
        comment3.Print();
        comment4.Print();
        comment5.Print();

        for (int i = 0; i < 5; i++)
            network.LikePost(post1.GetPostID());

        for (int i = 0; i < 3; i++)
            network.LikePost(post2.GetPostID());

        for (int i = 0; i < 4; i++)
            network.LikePost(post3.GetPostID());

        for (int i = 0; i < 2; i++)
            network.LikePost(post4.GetPostID());

        Console.WriteLine("Додано лайки:\n");
        Console.WriteLine($"Пост 1: {post1.GetLikes()} лайків");
        Console.WriteLine($"Пост 2: {post2.GetLikes()} лайків");
        Console.WriteLine($"Пост 3: {post3.GetLikes()} лайків");
        Console.WriteLine($"Пост 4: {post4.GetLikes()} лайків");

        var op1 = user.CreateOperation(
            Operation.OperationType.CreatePost,
            post1.GetPostID(),
            post1.GetContent(),
            0
        );

        var op2 = user.CreateOperation(
            Operation.OperationType.CreateComment,
            post1.GetPostID(),
            comment1.GetText(),
            0
        );

        var op3 = user.CreateOperation(
            Operation.OperationType.Like,
            post2.GetPostID(),
            "",
            0
        );

        Console.WriteLine($"Операція 1 (Пост): {(op1.VerifyOperation() ? "Валідна" : "Невалідна")}");
        Console.WriteLine($"Операція 2 (Коментар): {(op2.VerifyOperation() ? "Валідна" : "Невалідна")}");
        Console.WriteLine($"Операція 3 (Лайк): {(op3.VerifyOperation() ? "Валідна" : "Невалідна")}\n");

        var operations = new List<Operation> { op1, op2, op3 };
        var tx1 = new Transaction(operations, 1);

        Console.WriteLine($"ID Транзакції: {tx1.GetTransactionID().Substring(0, 20)}...");
        Console.WriteLine($"Операцій у транзакції: {tx1.GetOperations().Count}");
        Console.WriteLine($"Nonce: {tx1.GetNonce()}\n");

        var block1 = Block.CreateBlock(
            new List<Transaction> { tx1 },
            network.GetLastBlockID()
        );

        if (network.ValidateBlock(block1))
        {
            Console.WriteLine("Блок успішно валідований!");
            Console.WriteLine($"Блок додано до блокчейну!");
            Console.WriteLine($"ID Блока: {block1.GetBlockID().Substring(0, 20)}...");
            Console.WriteLine($"Час створення: {block1.GetTimestamp():yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Транзакцій у блоці: {block1.GetTransactionCount()}");
        }
        else
        {
            Console.WriteLine("Помилка при валідації блока!");
        }

        var op4 = user.CreateOperation(
            Operation.OperationType.CreateComment,
            post2.GetPostID(),
            "Ще один цікавий коментар до мого поста",
            0
        );

        var op5 = user.CreateOperation(
            Operation.OperationType.Like,
            post4.GetPostID(),
            "",
            0
        );

        var tx2 = new Transaction(new List<Operation> { op4, op5 }, 2);
        var block2 = Block.CreateBlock(
            new List<Transaction> { tx2 },
            network.GetLastBlockID()
        );

        if (network.ValidateBlock(block2))
        {
            Console.WriteLine("Другий блок успішно додано!");
        }

        var blockHistory = network.GetBlockHistory();
        for (int i = 0; i < blockHistory.Count; i++)
        {
            var blockId = blockHistory[i].GetBlockID().Substring(0, 16);
            var txCount = blockHistory[i].GetTransactionCount();
            var isGenesis = blockHistory[i].IsGenesis() ? " [ГЕНЕЗІС]" : "";
            Console.WriteLine($"  Блок #{i}: {blockId}... (Транзакцій: {txCount}){isGenesis}");
        }

        profile.PrintProfile();

        var posts = new List<Post> { post1, post2, post3, post4 };
        for (int i = 0; i < posts.Count; i++)
        {
            var post = posts[i];
            var comments = network.GetPostComments(post.GetPostID());
            Console.WriteLine($"Пост {i + 1}: {post.GetContent().Substring(0, Math.Min(40, post.GetContent().Length))}...");
            Console.WriteLine($"Лайків: {post.GetLikes()} | Коментарів: {comments.Count}\n");
        }
    }
}