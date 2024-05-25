using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Voting.Models;

public partial class VotingContext : DbContext
{
    public VotingContext()
    {
    }

    public VotingContext(DbContextOptions<VotingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidatesVoter> CandidatesVoters { get; set; }

    public virtual DbSet<Voter> Voters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
     }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.Property(e => e.CandidateId).HasColumnName("candidate_id");
            entity.Property(e => e.CandidateName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("candidate_name");
        });
        modelBuilder.Entity<Voter>(entity =>
        {
            entity.Property(e => e.VoterId).HasColumnName("voter_id");
            entity.Property(e => e.VoterName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("voter_name");
        });

        modelBuilder.Entity<CandidatesVoter>(entity =>
        {
            entity.HasKey(e => e.CandidatesvotersId);

            entity.Property(e => e.CandidatesvotersId).HasColumnName("candidatesvoters_id");
            entity.Property(e => e.CandidateId).HasColumnName("candidate_id");
            entity.Property(e => e.Voted).HasColumnName("voted");
            entity.Property(e => e.VoterId).HasColumnName("voter_id");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidatesVoters)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("FK__Candidate__candi__4E88ABD4");

            entity.HasOne(d => d.Voter).WithMany(p => p.CandidatesVoters)
                .HasForeignKey(d => d.VoterId)
                .HasConstraintName("FK__Candidate__voter__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
