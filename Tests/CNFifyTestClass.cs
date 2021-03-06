﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.FSharp.Collections;


namespace CNFifyTests
{
    [TestClass]
    public class CNFifyTestClass
    {
        public CNFify.Term a = CNFify.Term.NewVar("a");
        public CNFify.Term b = CNFify.Term.NewVar("b");
        public CNFify.Term c = CNFify.Term.NewVar("c");

        public CNFify.Term p = CNFify.Term.NewVar("p");

        public CNFify.Term q = CNFify.Term.NewVar("q");
        public CNFify.Term r = CNFify.Term.NewVar("r");

        [TestMethod]
        public void SimpleDistribution()
        {
            // p ∨ (q ∧ r) ----> (q ∨ p) ∧ (r ∨ p)

            var T = CNFify.Term.NewOr(p,CNFify.Term.NewAnd(q, r));

            var result = CNFify.normalize(T);

            var expected = CNFify.Term.NewAnd(CNFify.Term.NewOr(p, q), CNFify.Term.NewOr(p, r));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InnerDistribution()
        {
            // a ∧ (p ∨ (q ∧ r)) ----> a ∧ (q ∨ p) ∧ (r ∨ p)

            var T = CNFify.Term.NewAnd(a,CNFify.Term.NewOr(p, CNFify.Term.NewAnd(q, r)));

            var result = CNFify.normalize(T);

            var expected = CNFify.Term.NewAnd(a,CNFify.Term.NewAnd(CNFify.Term.NewOr(p, q), CNFify.Term.NewOr(p, r)));

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SimpleDeMorgan()
        {
            // ! (q ∧ r) ----> !q ∨ !r

            var T = CNFify.Term.NewNot(CNFify.Term.NewAnd(q, r));

            var result = CNFify.normalize(T);

            var expected = CNFify.Term.NewOr(CNFify.Term.NewNot(q), CNFify.Term.NewNot(r));

            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void SimpleEquality()
        {
            // (a <=> b) ----> (a ∨ !b) ∧ (b ∨ !a)

            var T = CNFify.equal(a, b);

            var result = CNFify.normalize(T);

            var expected = CNFify.Term.NewAnd(
                    CNFify.Term.NewOr(CNFify.Term.NewNot(a), b),
                    CNFify.Term.NewOr(CNFify.Term.NewNot(b), a)
                );

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SimpleRemoveNotBImp()
        {
            // !(a <=> b) ----> (a ∨ b) ∧ (!b ∨ !a)

            var T = CNFify.Term.NewNot(CNFify.equal(a, b));

            var result = CNFify.normalize(T);

            var expected = CNFify.Term.NewAnd(
                CNFify.Term.NewOr(b, a),
                CNFify.Term.NewOr(CNFify.Term.NewNot(a), CNFify.Term.NewNot(b))

                );

            Assert.AreEqual(expected, result);
        }


        

        //[TestMethod]
        //public void MakeAndList()
        //{
        //    // (a ∧ (b ∧ c)  ----> AndList (a; b; c) 

        //    var T = CNFify.Term.NewAnd(a, CNFify.Term.NewAnd(b,c));

        //    var result = CNFify.createAndClauseFromList(T);

        //    List<CNFify.Term> abcList = new List<CNFify.Term>() { a, b, c };

        //    FSharpList<CNFify.Term> expected = ListModule.OfSeq(abcList);

        //    Assert.AreEqual(expected, result);
        //}

        //[TestMethod]
        //public void MakeAndListWithOr()
        //{
        //    // (a ∧ (b ∨ c))  ----> AndList (a; b ∨ c) 

        //    var T = CNFify.Term.NewAnd(a, CNFify.Term.NewOr(b, c));

        //    var result = CNFify.createAndClauseFromList(T);

        //    List<CNFify.Term> abcList = new List<CNFify.Term>() { a, CNFify.Term.NewOr(b, c) };

        //    FSharpList<CNFify.Term> expected = ListModule.OfSeq(abcList);

        //    Assert.AreEqual(expected, result);
        //}


    }
}
