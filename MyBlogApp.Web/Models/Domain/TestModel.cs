using System;

namespace TestApplication.Models
{
    // Define a TestModel class
    public class TestModel
    {
        // Properties of the class
        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public string TestResult { get; set; }

        // Constructor for TestModel
        public TestModel(int testId, string testName, DateTime testDate, string testResult)
        {
            TestId = testId;
            TestName = testName;
            TestDate = testDate;
            TestResult = testResult;
        }

        // Method to display test details
        public void DisplayTestDetails()
        {
            Console.WriteLine("Test ID: " + TestId);
            Console.WriteLine("Test Name: " + TestName);
            Console.WriteLine("Test Date: " + TestDate.ToString("MM/dd/yyyy"));
            Console.WriteLine("Test Result: " + TestResult);
        }

        // Method to evaluate the test result
        public bool IsTestPassed()
        {
            return TestResult.Equals("Passed", StringComparison.OrdinalIgnoreCase);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Creating an instance of TestModel
            TestModel test = new TestModel(
                testId: 1,
                testName: "Sample Unit Test",
                testDate: DateTime.Now,
                testResult: "Passed"
            );

            // Display test details
            test.DisplayTestDetails();

            // Check if the test is passed
            Console.WriteLine("\nTest Passed: " + test.IsTestPassed());

            // Wait for the user to press a key before exiting
            Console.ReadLine();
        }
    }
}