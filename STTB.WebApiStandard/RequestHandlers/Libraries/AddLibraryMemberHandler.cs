using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Libraries;
using STTB.WebApiStandard.Contracts.ResponseModels.Libraries;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.Libraries
{
    public class AddLibraryMemberHandler : IRequestHandler<AddLibraryMemberRequest, AddLibraryMemberResponse>
    {
        private readonly SttbDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AddLibraryMemberHandler(SttbDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<AddLibraryMemberResponse> Handle(AddLibraryMemberRequest request, CancellationToken ct)
        {
            var libraryMember = new LibraryMember
            {
                FullName = request.FullName,
                Dob = DateOnly.Parse(request.DOB),
                InstitutionName = request.InstitutionName,
                Contact = request.Contact,
                Address = request.Address,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.LibraryMembers.AddAsync(libraryMember, ct);
            await _db.SaveChangesAsync(ct);

            await SaveImageAssetAsync(request.PassportImage!, libraryMember.Id, "passport_image", @"library_members\passport_image", ct);
            await SaveImageAssetAsync(request.IdImage!, libraryMember.Id, "id_image", @"library_members\id_image", ct);
            await SaveImageAssetAsync(request.ProofOfDepositImage!, libraryMember.Id, "proof_of_deposit_image", @"library_members\proof_of_deposit_image", ct);

            return new AddLibraryMemberResponse
            {
                MemberId = libraryMember.Id.ToString(),
                MemberName = libraryMember.FullName,
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
