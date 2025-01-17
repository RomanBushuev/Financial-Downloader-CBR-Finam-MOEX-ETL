﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataBaseLink;
using Core.Mir.BaseTypes;
using Core.Mir.Enumerations;
using DataProvider.Output.Mir.DbObject;

namespace DataProvider.Output.Mir.DbRepository
{
    public static class FisdItem
    {
        //find
        public static fisd_item FindId(DbLink dbLink, int fisd_id, DateTime dateTime)
        {
            string query =
                string.Format(@"select * from fisd_item t
                    where t.fisd_id = {0} and t.dat_from <= to_date('{1}', 'dd.mm.yyyy')",
                    fisd_id, dateTime.ToString("dd.MM.yyyy"));
            var result = dbLink.GetConnection().QueryFirstOrDefault<fisd_item>(query);
            return result;
        }

        //insert
        public static void Insert(DbLink dbLink, fisd_item fisd_item)
        {
            //при первой вставке дату указывать 01.01.1900

            if(FindId(dbLink, fisd_item.fisd_id, fisd_item.dat_from) == null)
            {
                fisd_item.dat_from = new DateTime(1900, 01, 01);
            }
            else
            {
                var result = FindId(dbLink, fisd_item.fisd_id, fisd_item.dat_from);
                if (fisd_item.val == result.val)
                    return;
            }
            string query =
                @"insert into fisd_item(val, dat_from, fisd_id)
                    values(@val, @dat_from, @fisd_id)";
            dbLink.GetConnection().Execute(query, fisd_item);
        }

        //remove
        public static void Remove(DbLink dbLink, int fisd_id, DateTime dateTime)
        {
            string query =
                string.Format(@"delete from fisd_item t where t.fisd_id = {0} and t.dat_from = to_date('{1}', 'dd.mm.yyyy')",
                    fisd_id, dateTime.ToString("dd.MM.yyyy"));
            dbLink.GetConnection().Execute(query);
        }
    }
}
