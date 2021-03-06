﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Class used to batch process atom entries for Insert, Update, and Delete
    /// </summary>
    public sealed class BatchProcess
    {
        /// <summary>
        /// The only instance of the BatchProcess class
        /// </summary>
        public static readonly BatchProcess Instance = new BatchProcess();

        private readonly IList<SDataBatchRequest> _requests;

        private BatchProcess()
        {
            _requests = new List<SDataBatchRequest>();
        }

        /// <summary>
        /// Current stack
        /// </summary>
        public IList<SDataBatchRequest> Requests
        {
            get { return _requests; }
        }

        /// <summary>
        /// Adds a url to the batch for processing
        /// </summary>
        /// <param name="item">url for batch item</param>
        /// <returns>True if an appropriate pending batch operation was found</returns>
        public bool AddToBatch(SDataBatchRequestItem item)
        {
            Guard.ArgumentNotNull(item, "item");

            var id = GetBatchKey(item.Url, "$batch");
            var request = _requests.LastOrDefault(x => string.Equals(GetBatchKey(x.ToString()), id, StringComparison.InvariantCultureIgnoreCase));

            if (request != null)
            {
                request.Items.Add(item);
                return true;
            }

            return false;
        }

        private static string GetBatchKey(string url, params string[] extraSegments)
        {
            return new SDataUri(url)
                   {
                       CollectionPredicate = null,
                       Query = null
                   }.AppendPath(extraSegments).ToString();
        }
    }
}