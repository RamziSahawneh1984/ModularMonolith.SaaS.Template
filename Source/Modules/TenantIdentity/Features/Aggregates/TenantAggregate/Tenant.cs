﻿using Modules.TenantIdentity.Features.Domain.TenantAggregate.Exceptions;
using Modules.TenantIdentity.Web.Shared.DTOs.Aggregates.Tenant;
using Shared.Features.DomainKernel;
using Shared.Features.DomainKernel.Exceptions;
using Shared.Kernel.BuildingBlocks.Auth;
using Shared.Kernel.BuildingBlocks.Auth.Roles;
using Shared.Kernel.BuildingBlocks.Auth.Service;

namespace Modules.TenantIdentity.Features.Domain.TenantAggregate
{
    public class Tenant : AggregateRoot
    {
        private readonly IAuthorizationService _authorizationService;

        public Tenant() { }

        public override Guid TenantId { get => base.TenantId; }
        public string Name { get; set; }
        public TenantStyling Styling { get; set; }
        public TenantSettings Settings { get; set; }
        public SubscriptionPlanType CurrentSubscriptionPlanType => tenantSubscriptions.MaxBy(s => s.PeriodEnd).SubscriptionPlanType;
        public IReadOnlyCollection<TenantMembership> Memberships => memberships.AsReadOnly();
        private List<TenantMembership> memberships = new List<TenantMembership>();
        public IReadOnlyCollection<TenantInvitation> Invitations => invitations.AsReadOnly();
        private List<TenantInvitation> invitations = new List<TenantInvitation>();
        public IReadOnlyCollection<TenantSubscription> TenantSubscriptions => tenantSubscriptions.AsReadOnly();
        private List<TenantSubscription> tenantSubscriptions = new List<TenantSubscription>();

        public static async Task<Tenant> CreateTenantWithAdminAsync(string name, Guid adminUserId)
        {
            return new Tenant
            {


            };
        }

        public void AddUser(Guid userId, TenantRole role)
        {

            TenantMembership tenantMembership;
            if ((tenantMembership = memberships.SingleOrDefault(m => m.UserId == userId)) is not null)
            {
                tenantMembership.Role = role;
            }
            else
            {
                memberships.Add(new TenantMembership(userId, role));
            }
        }

        public void ChangeRoleOfUser(Guid userId, TenantRole role)
        {

        }

        public void ChangeRoleOfMember(Guid userId, TenantRole newRole)
        {
            _authorizationService.ThrowIfUserIsNotInRole(TenantRole.Admin);

            if (CheckIfMember(userId) is false)
            {
                throw new MemberNotFoundException();
            }
        }

        public void RemoveUser(Guid userId)
        {
            _authorizationService.ThrowIfUserIsNotInRole(TenantRole.Admin);

            if (CheckIfMember(userId) is false)
            {
                throw new MemberNotFoundException();
            }

            memberships.Remove(memberships.Single(m => m.UserId == userId));
        }

        public void InviteUserToRole(Guid userId, TenantRole role)
        {
            _authorizationService.ThrowIfUserIsNotInRole(TenantRole.Admin);

            if (CheckIfMember(userId))
            {
                throw new UserIsAlreadyMemberException();
            }

            invitations.Add(new TenantInvitation { UserId = userId, Role = role });
        }

        public void AddSubscription(string stripeSubscriptionId, SubscriptionPlanType type, DateTime startDate, DateTime endDate, bool isTrial)
        {
            tenantSubscriptions.Add(new TenantSubscription
            {
                StripeSubscriptionId = stripeSubscriptionId,
                SubscriptionPlanType = type,
                PeriodStart = startDate,
                PeriodEnd = endDate,
            });
        }

        public async void DeleteTenantMembership(Guid membershipId)
        {
            var tenantMembership = Memberships.SingleOrDefault(t => t.Id == membershipId);
            if (tenantMembership == null)
            {
                throw new NotFoundException();
            }
        }

        public bool CheckIfMember(Guid userId)
        {
            return memberships.Any(membership => membership.UserId == userId);
        }

        public void ThrowIfUserCantDeleteTenant()
        {
            _authorizationService.ThrowIfUserIsNotInRole(TenantRole.Admin);
        }

        public TenantDTO ToDTO() => new TenantDTO();
        public TenantDetailDTO ToDetailDTO() => new TenantDetailDTO();

    }
}