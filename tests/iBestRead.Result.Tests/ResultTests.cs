using System.Collections.Generic;
using System.Linq;
using iBestRead.Results;
using Shouldly;
using Xunit;

namespace iBestRead.Result.Tests
{
    public class ResultTests
    {
        [Fact]
        public void InitializesStronglyTypedStringValue()
        {
            var expectedString = "test string";
            var result = new Result<string>(expectedString);
            result.Value.ShouldBe(expectedString);
        }

        [Fact]
        public void InitializesStronglyTypedIntValue()
        {
            var expectedInt = 123;
            var result = new Result<int>(expectedInt);
            result.Value.ShouldBe(expectedInt);
        }

        [Fact]
        public void InitializesStronglyTypedObjectValue()
        {
            var expectedObject = new TestObject();
            var result = new Result<TestObject>(expectedObject);
            result.Value.ShouldBe(expectedObject);
        }

        private class TestObject
        {
        }

        [Fact]
        public void InitializesValueToNullGivenNullConstructorArgument()
        {
            var result = new Result<object>(null);

            result.Value.ShouldBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(123)]
        [InlineData("test value")]
        public void InitializesStatusToOkGivenValue(object value)
        {
            var result = new Result<object>(value);
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(123)]
        [InlineData("test value")]
        public void InitializesValueUsingFactoryMethodAndSetsStatusToOk(object value)
        {
            var result = Result<object>.Success(value);
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(value);
        }

        [Fact]
        public void InitializesStatusToErrorGivenErrorFactoryCall()
        {
            var result = Result<object>.Error();
            result.Status.ShouldBe(ResultStatus.Error);
        }

        [Fact]
        public void InitializesStatusToErrorAndSetsErrorMessageGivenErrorFactoryCall()
        {
            var errorMessage = "Something bad happened.";
            var result = Result<object>.Error(errorMessage);
            result.Status.ShouldBe(ResultStatus.Error);
            result.Errors.First().ShouldBe(errorMessage);
        }

        [Fact]
        public void InitializesStatusToInvalidAndSetsErrorMessagesGivenInvalidFactoryCall()
        {
            var validationErrors = new List<ValidationError>
            {
                new ValidationError
                {
                    Identifier = "name",
                    ErrorMessage = "Name is required"
                },
                new ValidationError
                {
                    Identifier = "name",
                    ErrorMessage = "Another name is required"
                },
                new ValidationError
                {
                    Identifier = "postalCode",
                    ErrorMessage = "PostalCode cannot exceed 10 characters"
                }
            };

            var result = Result<object>.Invalid(validationErrors);

            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ValidationErrors.ShouldContain(e =>
                e.Identifier == "name" && e.ErrorMessage == "Name is required");
            result.ValidationErrors.ShouldContain(e =>
                e.Identifier == "postalCode" && e.ErrorMessage == "PostalCode cannot exceed 10 characters");
        }

        [Fact]
        public void InitializesStatusToNotFoundGivenNotFoundFactoryCall()
        {
            var result = Result<object>.NotFound();
            result.Status.ShouldBe(ResultStatus.NotFound);
        }

        [Fact]
        public void InitializesStatusToForbiddenGivenForbiddenFactoryCall()
        {
            var result = Result<object>.Forbidden();
            result.Status.ShouldBe(ResultStatus.Forbidden);
        }
    }
}