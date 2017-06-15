﻿﻿﻿﻿﻿﻿using System;
using Xunit;
using JwtBuilder;

namespace JwtBuilder.UnitTests
{
    public class TokenBuilderTests
    {
        public TokenBuilderTests()
        {
            testConfiguration = new TokenConfiguration()
            {
                SigningKey = "c2VjcmV0",
                Audience = "http://example.com",
                Issuer = "test",
                BearerTokenLength = TimeSpan.FromMinutes(10),
                RefreshTokenLength = TimeSpan.FromMinutes(100)
                    
            };

            builder = new TokenBuilder(testConfiguration);
        }

        ITokenConfiguration testConfiguration;

        TokenBuilder builder;

        [Fact]
        public void Claim_Added()
        {
            builder.WithClaim(ClaimKey.Subject, "Laurence");

            Assert.Equal(builder.payload[ClaimKey.Subject], "Laurence");
            
        }

        [Fact]
        public void Duplicate_Claim_Throws_InvalidOperationException()
        {
            builder.WithClaim(ClaimKey.Subject, "Laurence");

            Assert.Throws<InvalidOperationException>(() =>
            {
                builder.WithClaim(ClaimKey.Subject, "Laurence");
            });
        }

        [Fact]
        public void Bearer_Token_Contains_Automatic_Items()
        {
            builder.WithClaims(
                ClaimKey.Audience,
                ClaimKey.Issuer
                );

            var spec = new BearerTokenSpec();

            Assert.Equal(builder.GetClaimValue<string>(ClaimKey.Audience, spec), 
                testConfiguration.GetAudience());

            Assert.Equal(builder.GetClaimValue<string>(ClaimKey.Issuer, spec), 
                testConfiguration.GetIssuer());
        }

        [Fact]
        public void Token_Expiry_Is_Within_Tolerance()
        {
            var expectedBearerTokenExpireTime = DateTime.UtcNow.AddMinutes(10).ToUnixTimestamp();
            var expectedRefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(100).ToUnixTimestamp();

            double tolerance = 100;

            builder.WithClaims(
                ClaimKey.Expires);

            var actualBearerTokenExpireTime = builder.GetClaimValue<double>(ClaimKey.Expires, new BearerTokenSpec());
            var actualRefreshTokenExpireTime = builder.GetClaimValue<double>(ClaimKey.Expires, new RefreshTokenSpec());

            Assert.InRange(actualBearerTokenExpireTime, expectedBearerTokenExpireTime - tolerance, expectedBearerTokenExpireTime + tolerance);
            Assert.InRange(actualRefreshTokenExpireTime, expectedRefreshTokenExpireTime - tolerance, expectedRefreshTokenExpireTime + tolerance);
        }

        [Fact]
        public void Token_Issued_At_Within_Tolerance()
        {
            var expectedIssuedAtTime = DateTime.UtcNow.ToUnixTimestamp();

            double tolerance = 100;

            builder.WithClaims(
                ClaimKey.IssuedAt);

            var actualIssuedAtTime = builder.GetClaimValue<double>(ClaimKey.IssuedAt, new BearerTokenSpec());

            Assert.InRange(actualIssuedAtTime, expectedIssuedAtTime - tolerance, expectedIssuedAtTime + tolerance);

        }

        [Fact]
        public void Custom_Claims_Merged_Correctly()
        {
            TokenBuilder builder = this.builder.WithCustomClaim(
                name: "Property1",
                value: "Value1") as TokenBuilder;

            Assert.True(builder
                .payload
                .ContainsKey("Property1"));
        }

        [Fact]
        public void Duplicate_Claims_Throw_ArgumentException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.builder.WithCustomClaim(
                        name: "Property1",
                        value: "Value1")
                    .WithCustomClaim(
                        name: "Property1",
                        value: "Value1");
            });
        }

        [Fact]
        public void Invalid_Standard_Claim_Throws_Exception()
        {
            Assert.Throws<Exception>(() =>
            {
                new TokenBuilder(testConfiguration)
                .WithClaim(
                    ClaimKey.Subject)
                    .GenerateToken(new BearerTokenSpec());
            });
        }

        [Fact]
        public void Bearer_Token_Is_Correct()
        {
            var testConfiguration = new TokenConfiguration()
            {
                SigningKey = "c2VjcmV0"
            };

            var builder = new TokenBuilder(testConfiguration)
                .WithClaim(ClaimKey.Subject, "1234567890")
                .WithCustomClaim("name", "John Doe")
                .WithCustomClaim("admin", true);

            var tokenResult = builder.GenerateToken(new BearerTokenSpec());

            Assert.True(tokenResult.IsSuccess);

            Assert.Equal(tokenResult.Value.ToString(),
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ");
        }

        [Fact]
        public void Retreiving_Claim_Value_Correct()
        {
            Assert.Equal((builder.WithClaim(ClaimKey.Subject, "Laurence") as TokenBuilder)
                .GetClaimValue<string>(ClaimKey.Subject, new BearerTokenSpec()), "Laurence");
        }

    }
}
