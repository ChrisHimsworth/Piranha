﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Piranha.Models;

namespace Piranha.Web
{
	/// <summary>
	/// HtmlHelper extensions for the Piranha application.
	/// </summary>
	public static class PiranhaHelper
	{
		/// <summary>
		/// Url helper for generating the url for the given permalink.
		/// </summary>
		/// <param name="helper">The url helper</param>
		/// <param name="permalink">The permalink</param>
		/// <param name="draft">Whether to generate a link to the draft</param>
		/// <returns>An action url</returns>
		public static string GetPermalink(this UrlHelper helper, string permalink, bool draft = false) {
			if (draft)
				return helper.Content("~/" + WebPages.WebPiranha.GetUrlPrefixForHandlerId("DRAFT") + "/" + permalink.ToLower()) ;
			return helper.Content("~/" + (!WebPages.WebPiranha.PrefixlessPermalinks ? 
				WebPages.WebPiranha.GetUrlPrefixForHandlerId("PERMALINK").ToLower() + "/" : "") + permalink.ToLower()) ;
		}

		/// <summary>
		/// Generates an image tag for the specified thumbnail.
		/// </summary>
		/// <param name="id">The content id</param>
		/// <param name="size">Optional size</param>
		/// <returns>The url</returns>
		public static string GetThumbnailUrl(this UrlHelper helper, Guid id, int size = 0) {
			Content cnt = Models.Content.GetSingle(id, true) ;
			
			if (cnt != null) {
				var thumbId = cnt.IsImage ? id : (cnt.IsFolder ? Drawing.Thumbnails.GetIdByType("folder") : Drawing.Thumbnails.GetIdByType(cnt.Type)) ;

				return helper.Content("~/" + 
					WebPages.WebPiranha.GetUrlPrefixForHandlerId("THUMBNAILDRAFT") + "/" + 
					thumbId.ToString() + (size > 0 ? "/" + size.ToString() : "")) ;
			}
			return "" ;
		}

		/// <summary>
		/// Gets the URL to the content with the given id.
		/// </summary>
		/// <param name="id">The content id</param>
		/// <param name="width">Optional width</param>
		/// <param name="height">Optional height</param>
		/// <returns>The content url</returns>
		public static string GetContentUrl(this UrlHelper helper, Guid id, int width = 0, int height = 0) {
			Content cnt = Models.Content.GetSingle(id, true) ;
			
			if (cnt != null)
				return helper.Content("~/" + WebPages.WebPiranha.GetUrlPrefixForHandlerId("CONTENT") +
					"/" + id.ToString() + (width > 0 ? "/" + width.ToString() : "")) + (height > 0 ? "/" + height.ToString() : "") ;
			return "" ;
		}

		/// <summary>
		/// Gets the image URL for the gravatar with the given email
		/// </summary>
		/// <param name="email">The gravatar email</param>
		/// <param name="size">Optional size</param>
		/// <returns>The image URL</returns>
		public static string GetGravatarUrl(this UrlHelper helper, string email, int size = 0) {
			var input = UTF8Encoding.UTF8.GetBytes(email) ;
			var crypto = new MD5CryptoServiceProvider() ;

			var hash = Convert.ToBase64String(crypto.TransformFinalBlock(input, 0, input.Length)) ;
			crypto.Clear() ;

			return "http://www.gravatar.com/avatar/" + hash.ToLower() +
				(size > 0 ? "?s=" + size : "") ;
		}

		/// <summary>
		/// Gets an encrypted API-key valid for 30 minutes.
		/// </summary>
		/// <param name="apiKey">The API-key</param>
		/// <returns>The ecnrypted key</returns>
		public static string APIKey(this HtmlHelper helper, Guid apiKey) {
			return HttpUtility.UrlEncode(APIKeys.EncryptApiKey(apiKey)) ;
		}

		/// <summary>
		/// Gets an encrypted API-key valid for 30 minutes.
		/// </summary>
		/// <param name="apiKey">The API-key</param>
		/// <returns>The ecnrypted key</returns>
		public static string APIKey(this HtmlHelper helper, string apiKey) {
			return APIKey(helper, new Guid(apiKey)) ;
		}

		/// <summary>
		/// Generats the base url for all thumbnails.
		/// </summary>
		/// <param name="helper">The url helper</param>
		/// <returns>The base url</returns>
		public static string GetThumbnailBaseUrl(this UrlHelper helper) {
			return helper.Content("~/" + WebPages.WebPiranha.GetUrlPrefixForHandlerId("THUMBNAIL")) ;
		}
	}
}
