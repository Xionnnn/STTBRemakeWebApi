using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Donations;
using STTB.WebApiStandard.Contracts.ResponseModels.Donations;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.Web.Donations
{
    public class AddDonorMemberHandler : IRequestHandler<AddDonorMemberRequest, AddDonorMemberResponse>
    {
        private readonly SttbDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AddDonorMemberHandler(SttbDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<AddDonorMemberResponse> Handle(AddDonorMemberRequest request, CancellationToken ct)
        {
            var donorMember = new DonorMember
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Salutation = request.Salutation,
                Contact = request.Contact,
                Address = request.Address,
                Email = request.Email,
                DonationType = request.DonationType,
                DonationArea = request.DonationArea,
                ProofOfSupport = request.ProofOfSupport,
                DonationAmount = request.DonationAmount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.DonorMembers.AddAsync(donorMember, ct);
            await _db.SaveChangesAsync(ct);

            if (request.DonationType == "beasiswa" && request.StudentName != null && request.AcademicProgramId != null)
            {
                var scholarshipDetail = new DonorScholarshipDetail
                {
                    DonorMemberId = donorMember.Id,
                    StudentName = request.StudentName,
                    AcademicProgramId = request.AcademicProgramId.Value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _db.DonorScholarshipDetails.AddAsync(scholarshipDetail, ct);
                await _db.SaveChangesAsync(ct);
            }

            await SaveImageAssetAsync(request.ProofOfDonationImage!, donorMember.Id, "donation_proof_image", @"donor_members\donation_proof_image", ct);

            return new AddDonorMemberResponse
            {
                DonorId = donorMember.Id.ToString(),
                DonorName = $"{donorMember.FirstName} {donorMember.LastName}",
                IsSuccess = true
            };
        }

        private async Task SaveImageAssetAsync(IFormFile file, long memberId, string columnName, string modelType, CancellationToken ct)
        {
            var extension = Path.GetExtension(file.FileName);
            var uuid = Guid.NewGuid().ToString();
            var fileName = $"{uuid}{extension}";
            var relativePath = $"uploads/images/{columnName}/{fileName}";

            var physicalDir = Path.Combine(_env.WebRootPath, "uploads", "images", columnName);
            Directory.CreateDirectory(physicalDir);

            var physicalPath = Path.Combine(physicalDir, fileName);
            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            var asset = new Asset
            {
                FileName = fileName,
                FilePath = relativePath,
                MimeType = file.ContentType,
                SizeBytes = file.Length,
                ModelType = modelType,
                ModelId = memberId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Assets.AddAsync(asset, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
