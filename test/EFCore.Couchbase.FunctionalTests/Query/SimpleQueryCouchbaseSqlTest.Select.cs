// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Couchbase.Query
{
    public partial class SimpleQueryCouchbaseTest
    {
        public override async Task Projection_when_arithmetic_expression_precendence(bool isAsync)
        {
            await base.Projection_when_arithmetic_expression_precendence(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Projection_when_arithmetic_expressions(bool isAsync)
        {
            await base.Projection_when_arithmetic_expressions(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Projection_when_arithmetic_mixed(bool isAsync)
        {
            await base.Projection_when_arithmetic_mixed(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Projection_when_arithmetic_mixed_subqueries(bool isAsync)
        {
            await base.Projection_when_arithmetic_mixed_subqueries(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Projection_when_null_value(bool isAsync)
        {
            await base.Projection_when_null_value(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Projection_when_client_evald_subquery(bool isAsync)
        {
            await base.Projection_when_client_evald_subquery(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Project_to_object_array(bool isAsync)
        {
            await base.Project_to_object_array(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""EmployeeID""] = 1))");
        }

        public override async Task Project_to_int_array(bool isAsync)
        {
            await base.Project_to_int_array(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Employee"") AND (c[""EmployeeID""] = 1))");
        }

        public override async Task Select_bool_closure_with_order_by_property_with_cast_to_nullable(bool isAsync)
        {
            await base.Select_bool_closure_with_order_by_property_with_cast_to_nullable(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_bool_closure_with_order_parameter_with_cast_to_nullable(bool isAsync)
        {
            await base.Select_bool_closure_with_order_parameter_with_cast_to_nullable(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_scalar(bool isAsync)
        {
            await base.Select_scalar(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_anonymous_one(bool isAsync)
        {
            await base.Select_anonymous_one(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_anonymous_two(bool isAsync)
        {
            await base.Select_anonymous_two(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_anonymous_three(bool isAsync)
        {
            await base.Select_anonymous_three(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_anonymous_bool_constant_true(bool isAsync)
        {
            await base.Select_anonymous_bool_constant_true(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_anonymous_constant_in_expression(bool isAsync)
        {
            await base.Select_anonymous_constant_in_expression(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_anonymous_conditional_expression(bool isAsync)
        {
            await base.Select_anonymous_conditional_expression(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Product"")");
        }

        public override async Task Select_constant_int(bool isAsync)
        {
            await base.Select_constant_int(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_constant_null_string(bool isAsync)
        {
            await base.Select_constant_null_string(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_local(bool isAsync)
        {
            await base.Select_local(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_scalar_primitive_after_take(bool isAsync)
        {
            await base.Select_scalar_primitive_after_take(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Employee"")");
        }

        public override async Task Select_project_filter(bool isAsync)
        {
            await base.Select_project_filter(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""City""] = ""London""))");
        }

        public override async Task Select_project_filter2(bool isAsync)
        {
            await base.Select_project_filter2(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""City""] = ""London""))");
        }

        public override async Task Select_nested_collection(bool isAsync)
        {
            await base.Select_nested_collection(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""City""] = ""London""))");
        }

        public override void Select_nested_collection_multi_level()
        {
            base.Select_nested_collection_multi_level();

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_nested_collection_multi_level2()
        {
            base.Select_nested_collection_multi_level2();

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_nested_collection_multi_level3()
        {
            base.Select_nested_collection_multi_level3();

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_nested_collection_multi_level4()
        {
            base.Select_nested_collection_multi_level4();

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override void Select_nested_collection_multi_level5()
        {
            using (var context = CreateContext())
            {
                var customers = context.Customers
                    .Where(c => c.CustomerID == "ALFKI")
                    .Select(
                        c => new
                        {
                            Order = (int?)c.Orders
                                .Where(o => o.OrderID < 10500)
                                .Select(
                                    o => o.OrderDetails
                                        .Where(od => od.OrderID != c.Orders.Count)
                                        .Select(od => od.ProductID)
                                        .FirstOrDefault())
                                .FirstOrDefault()
                        })
                    .ToList();

                Assert.Equal(1, customers.Count);
                Assert.Equal(0, customers.Count(c => c.Order != null && c.Order != 0));
            }

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override void Select_nested_collection_multi_level6()
        {
            base.Select_nested_collection_multi_level6();

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_nested_collection_count_using_anonymous_type(bool isAsync)
        {
            await base.Select_nested_collection_count_using_anonymous_type(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task New_date_time_in_anonymous_type_works(bool isAsync)
        {
            await base.New_date_time_in_anonymous_type_works(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_non_matching_value_types_int_to_long_introduces_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_int_to_long_introduces_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_nullable_int_to_long_introduces_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_nullable_int_to_long_introduces_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_nullable_int_to_int_doesnt_introduce_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_nullable_int_to_int_doesnt_introduce_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_int_to_nullable_int_doesnt_introduce_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_int_to_nullable_int_doesnt_introduce_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_from_binary_expression_introduces_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_from_binary_expression_introduces_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_from_binary_expression_nested_introduces_top_level_explicit_cast(
            bool isAsync)
        {
            await base.Select_non_matching_value_types_from_binary_expression_nested_introduces_top_level_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_from_unary_expression_introduces_explicit_cast1(bool isAsync)
        {
            await base.Select_non_matching_value_types_from_unary_expression_introduces_explicit_cast1(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_from_unary_expression_introduces_explicit_cast2(bool isAsync)
        {
            await base.Select_non_matching_value_types_from_unary_expression_introduces_explicit_cast2(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_from_length_introduces_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_from_length_introduces_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_from_method_call_introduces_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_from_method_call_introduces_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_non_matching_value_types_from_anonymous_type_introduces_explicit_cast(bool isAsync)
        {
            await base.Select_non_matching_value_types_from_anonymous_type_introduces_explicit_cast(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Select_conditional_with_null_comparison_in_test(bool isAsync)
        {
            await base.Select_conditional_with_null_comparison_in_test(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Projection_in_a_subquery_should_be_liftable(bool isAsync)
        {
            await base.Projection_in_a_subquery_should_be_liftable(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Employee"")");
        }

        public override async Task Projection_containing_DateTime_subtraction(bool isAsync)
        {
            await base.Projection_containing_DateTime_subtraction(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""OrderID""] < 10300))");
        }

        public override async Task Project_single_element_from_collection_with_OrderBy_Take_and_FirstOrDefault(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID == "ALFKI").Select(
                    c => c.Orders.OrderBy(o => o.OrderID).Select(o => o.CustomerID).Take(1).FirstOrDefault()));

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Project_single_element_from_collection_with_OrderBy_Skip_and_FirstOrDefault(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID == "ALFKI").Select(
                    c => c.Orders.OrderBy(o => o.OrderID).Select(o => o.CustomerID).Skip(1).FirstOrDefault()));

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Project_single_element_from_collection_with_OrderBy_Distinct_and_FirstOrDefault(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID == "ALFKI").Select(
                    c => c.Orders.OrderBy(o => o.OrderID).Select(o => o.CustomerID).Distinct().FirstOrDefault()));

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Project_single_element_from_collection_with_OrderBy_Take_and_SingleOrDefault(bool isAsync)
        {
            await base.Project_single_element_from_collection_with_OrderBy_Take_and_SingleOrDefault(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Project_single_element_from_collection_with_OrderBy_Take_and_FirstOrDefault_with_parameter(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID == "ALFKI").Select(
                    c => c.Orders.OrderBy(o => o.OrderID).Select(o => o.CustomerID).Take(1).FirstOrDefault()));

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Project_single_element_from_collection_with_multiple_OrderBys_Take_and_FirstOrDefault(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID == "ALFKI").Select(
                    c => c.Orders.OrderBy(o => o.OrderID)
                        .ThenByDescending(o => o.OrderDate)
                        .Select(o => o.CustomerID)
                        .Take(2)
                        .FirstOrDefault()));

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task
            Project_single_element_from_collection_with_multiple_OrderBys_Take_and_FirstOrDefault_followed_by_projection_of_length_property(
                bool isAsync)
        {
            await base
                .Project_single_element_from_collection_with_multiple_OrderBys_Take_and_FirstOrDefault_followed_by_projection_of_length_property(
                    isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Project_single_element_from_collection_with_multiple_OrderBys_Take_and_FirstOrDefault_2(bool isAsync)
        {
            await AssertQuery<Customer>(
                isAsync,
                cs => cs.Where(c => c.CustomerID == "ALFKI").Select(
                    c => c.Orders.OrderBy(o => o.CustomerID)
                        .ThenByDescending(o => o.OrderDate)
                        .Select(o => o.CustomerID)
                        .Take(2)
                        .FirstOrDefault()));

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Customer"") AND (c[""CustomerID""] = ""ALFKI""))");
        }

        public override async Task Project_single_element_from_collection_with_OrderBy_over_navigation_Take_and_FirstOrDefault(bool isAsync)
        {
            await AssertQueryScalar<Order>(
                isAsync,
                os => os.Where(o => o.OrderID < 10250)
                    .Select(
                        o => o.OrderDetails.OrderBy(od => od.Product.ProductName).Select(od => od.OrderID).Take(1).FirstOrDefault()));

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""OrderID""] < 10250))");
        }

        public override async Task Project_single_element_from_collection_with_OrderBy_over_navigation_Take_and_FirstOrDefault_2(
            bool isAsync)
        {
            await base.Project_single_element_from_collection_with_OrderBy_over_navigation_Take_and_FirstOrDefault_2(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (c[""OrderID""] < 10250))");
        }

        public override async Task Select_datetime_year_component(bool isAsync)
        {
            await base.Select_datetime_year_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_datetime_month_component(bool isAsync)
        {
            await base.Select_datetime_month_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_datetime_day_of_year_component(bool isAsync)
        {
            await base.Select_datetime_day_of_year_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_datetime_day_component(bool isAsync)
        {
            await base.Select_datetime_day_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_datetime_hour_component(bool isAsync)
        {
            await base.Select_datetime_hour_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_datetime_minute_component(bool isAsync)
        {
            await base.Select_datetime_minute_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_datetime_second_component(bool isAsync)
        {
            await base.Select_datetime_second_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_datetime_millisecond_component(bool isAsync)
        {
            await base.Select_datetime_millisecond_component(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_byte_constant(bool isAsync)
        {
            await base.Select_byte_constant(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_short_constant(bool isAsync)
        {
            await base.Select_short_constant(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Select_bool_constant(bool isAsync)
        {
            await base.Select_bool_constant(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_projection_AsNoTracking_Selector(bool isAsync)
        {
            await base.Anonymous_projection_AsNoTracking_Selector(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Anonymous_projection_with_repeated_property_being_ordered(bool isAsync)
        {
            await base.Anonymous_projection_with_repeated_property_being_ordered(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }

        public override async Task Anonymous_projection_with_repeated_property_being_ordered_2(bool isAsync)
        {
            await base.Anonymous_projection_with_repeated_property_being_ordered_2(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_GetValueOrDefault_on_DateTime(bool isAsync)
        {
            await base.Select_GetValueOrDefault_on_DateTime(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Order"")");
        }

        public override async Task Select_GetValueOrDefault_on_DateTime_with_null_values(bool isAsync)
        {
            await base.Select_GetValueOrDefault_on_DateTime_with_null_values(isAsync);

            AssertSql(
                @"SELECT c
FROM root c
WHERE (c[""Discriminator""] = ""Customer"")");
        }
    }
}
