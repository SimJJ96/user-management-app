using FluentValidation.TestHelper;
using UserManagement.API.DTOs;
using UserManagement.API.Validators;

namespace UserManagement.Tests.Validators
{
    public class UpdateUserDtoValidatorTests
    {
        private readonly UpdateUserDtoValidator _validator;

        public UpdateUserDtoValidatorTests()
        {
            _validator = new UpdateUserDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            var dto = new UpdateUserDto { FirstName = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Exceeds_Max_Length()
        {
            var dto = new UpdateUserDto { FirstName = new string('A', 51) };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Has_Invalid_Characters()
        {
            var dto = new UpdateUserDto { FirstName = "John123!" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Not_Have_Error_When_FirstName_Is_Valid()
        {
            var dto = new UpdateUserDto { FirstName = "John O'Neil" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        // LastName tests

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Empty()
        {
            var dto = new UpdateUserDto { LastName = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Exceeds_Max_Length()
        {
            var dto = new UpdateUserDto { LastName = new string('B', 51) };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Has_Invalid_Characters()
        {
            var dto = new UpdateUserDto { LastName = "Doe123!" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Not_Have_Error_When_LastName_Is_Valid()
        {
            var dto = new UpdateUserDto { LastName = "Doe-Smith" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        // Email tests

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            var dto = new UpdateUserDto { Email = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var dto = new UpdateUserDto { Email = "not-an-email" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Email_Is_Valid()
        {
            var dto = new UpdateUserDto { Email = "user@example.com" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        // PhoneNumber tests (optional)

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
        {
            var dto = new UpdateUserDto { PhoneNumber = "abc123" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Empty()
        {
            var dto = new UpdateUserDto { PhoneNumber = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
        {
            var dto = new UpdateUserDto { PhoneNumber = "+1234567890" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        // ZipCode tests (optional)

        [Fact]
        public void Should_Have_Error_When_ZipCode_Is_Invalid()
        {
            var dto = new UpdateUserDto { ZipCode = "!@#" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.ZipCode);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ZipCode_Is_Empty()
        {
            var dto = new UpdateUserDto { ZipCode = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ZipCode_Is_Valid()
        {
            var dto = new UpdateUserDto { ZipCode = "12345" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
        }
    }
}

