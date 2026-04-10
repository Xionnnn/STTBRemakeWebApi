using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Libraries;
using STTB.WebApiStandard.Contracts.ResponseModels.Libraries;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.Web.Libraries
{
    public class AddLibraryMemberHandler : IRequestHandler<AddLibraryMemberRequest, AddLibraryMemberResponse>
    {
        private readonly SttbDbContext _db;

        public AddLibraryMemberHandler(SttbDbContext db)
        {
            _db = db;
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

            await SaveImageAssetAsync(request.PassportImage!, libraryMember.Id, @"library_members\passport_image", ct);
            await SaveImageAssetAsync(request.IdImage!, libraryMember.Id, @"library_members\id_image", ct);
            await SaveImageAssetAsync(request.ProofOfDepositImage!, libraryMember.Id, @"library_members\proof_of_deposit_image", ct);

            return new AddLibraryMemberResponse
            {
                MemberId = libraryMember.Id.ToString(),
                MemberName = libraryMember.FullName,
                IsSuccess = true
            };
        }

        private async Task SaveImageAssetAsync(IFormFile file, long memberId, string modelType, CancellationToken ct)
        {
            var extension = Path.GetExtension(file.FileName);
            var uuid = Guid.NewGuid().ToString();
            var fileName = $"{uuid}{extension}";
            var relativePath = $"/Uploads/images/library_members/{fileName}";

            var physicalDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images", "library_members");
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
