using System;
using System.Collections.Generic;

namespace TestApplication.Models
{
    // A more detailed TestModel
    public class TestModel
    {
        // Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Constructor
        public TestModel(int id, string name, DateTime startTime, DateTime endTime)
        {
            Id = id;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Questions = new List<Question>();
        }

        // Method to Add Questions
        public void AddQuestion(int questionId, string text, string correctAnswer)
        {
            Questions.Add(new Question(questionId, text, correctAnswer));
        }

        // Method to Evaluate Test
        public int Evaluate(Dictionary<int, string> answers)
        {
            int score = 0;

            foreach (var question in Questions)
            {
                if (answers.ContainsKey(question.Id) && answers[question.Id].ToLower() == question.CorrectAnswer.ToLower())
                {
                    score++;
                }
            }

            return score;
        }

        // Display Test Summary
        public void DisplaySummary()
        {
            Console.WriteLine($"Test ID: {Id}");
            Console.WriteLine($"Test Name: {Name}");
            Console.WriteLine($"Start Time: {StartTime}");
            Console.WriteLine($"End Time: {EndTime}");
            Console.WriteLine($"Number of Questions: {Questions.Count}");
            Console.WriteLine();
        }
    }

    // Question class to represent each test question
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }

        public Question(int id, string text, string correctAnswer)
        {
            Id = id;
            Text = text;
            CorrectAnswer = correctAnswer;
        }
    }

    // Main Program to test the model
    class Program
    {
        static void Main(string[] args)
        {
            // Create a TestModel object
            TestModel test = new TestModel(1, "Math Quiz", DateTime.Now.AddHours(-1), DateTime.Now);

            // Add some questions
            test.AddQuestion(1, "What is 5 + 3?", "8");
            test.AddQuestion(2, "What is 12 - 7?", "5");
            test.AddQuestion(3, "What is 3 * 4?", "12");

            // Display the test summary
            test.DisplaySummary();

            // Simulate answers provided by a student
            var studentAnswers = new Dictionary<int, string>
            {
                { 1, "8" }, // Correct
                { 2, "5" }, // Correct
                { 3, "9" }  // Incorrect
            };

            // Evaluate and display the score
            int score = test.Evaluate(studentAnswers);
            Console.WriteLine($"Student's Score: {score}/{test.Questions.Count}");

            // Wait for user to close
            Console.ReadLine();
        }
    }
}