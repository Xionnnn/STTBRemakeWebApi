using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers
{
    public class DeleteLecturerRequest : IRequest<DeleteLecturerResponse>
    {
        public long Id { get; set; }
    }
}
