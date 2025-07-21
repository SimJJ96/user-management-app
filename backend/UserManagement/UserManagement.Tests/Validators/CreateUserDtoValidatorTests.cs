using FluentValidation.TestHelper;
using UserManagement.API.DTOs;
using UserManagement.API.Validators;

namespace UserManagement.Tests.Validators
{
    public class CreateUserDtoValidatorTests
    {
        private readonly CreateUserDtoValidator _validator;

        public CreateUserDtoValidatorTests()
        {
            _validator = new CreateUserDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            var dto = new CreateUserDto { FirstName = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Exceeds_Max_Length()
        {
            var dto = new CreateUserDto { FirstName = new string('A', 51) };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Has_Invalid_Characters()
        {
            var dto = new CreateUserDto { FirstName = "John123!" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Not_Have_Error_When_FirstName_Is_Valid()
        {
            var dto = new CreateUserDto { FirstName = "John O'Neil" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Empty()
        {
            var dto = new CreateUserDto { LastName = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Exceeds_Max_Length()
        {
            var dto = new CreateUserDto { LastName = new string('B', 51) };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Has_Invalid_Characters()
        {
            var dto = new CreateUserDto { LastName = "Doe123!" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Not_Have_Error_When_LastName_Is_Valid()
        {
            var dto = new CreateUserDto { LastName = "Doe-Smith" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        // Email tests

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            var dto = new CreateUserDto { Email = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var dto = new CreateUserDto { Email = "not-an-email" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Email_Is_Valid()
        {
            var dto = new CreateUserDto { Email = "user@example.com" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        // PhoneNumber tests (optional field)

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
        {
            var dto = new CreateUserDto { PhoneNumber = "12345abc" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Empty()
        {
            var dto = new CreateUserDto { PhoneNumber = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
        {
            var dto = new CreateUserDto { PhoneNumber = "+12345678901" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Have_Error_When_ZipCode_Is_Invalid()
        {
            var dto = new CreateUserDto { ZipCode = "!@#$" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.ZipCode);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ZipCode_Is_Empty()
        {
            var dto = new CreateUserDto { ZipCode = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ZipCode_Is_Valid()
        {
            var dto = new CreateUserDto { ZipCode = "12345-6789" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
        }
    }
}

