using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using OzdamarDepo.Domain.Employees;
using TS.Result;

namespace OzdamarDepo.Application.Employees
{
    public sealed record EmployeeCreateCommand(
        string FirstName,
        string LastName,
        DateOnly BirthOfDate,
        decimal Salary,
        PersonelInformation PersonelInformation,
        Address? Address) : IRequest<Result<string>>;

    public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
    {
        public EmployeeCreateCommandValidator()
        {
            RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır!");
            RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyadı alanı en az 3 karakter olmalıdır!");
            RuleFor(x => x.PersonelInformation.TCNo)
                .Length(11).WithMessage("TC numarası 11 haneli olmalıdır!")
                .Matches("^[0-9]*$").WithMessage("TC numarası sadece rakamlardan oluşmalıdır!");
        }
    }

    internal sealed class EmployeeCreateCommandHandler(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
        {
            bool isEmployeeExist = await employeeRepository.AnyAsync(
                p => p.PersonelInformation.TCNo == request.PersonelInformation.TCNo, 
                cancellationToken);

            if (isEmployeeExist)
            {
                return Result<string>.Failure("Bu TC numarası zaten kayıtlı!");
            }

            Employee employee = request.Adapt<Employee>();
            await employeeRepository.AddAsync(employee, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Personel kaydı başarıyla tamamlandı!");
        }
    }
} 