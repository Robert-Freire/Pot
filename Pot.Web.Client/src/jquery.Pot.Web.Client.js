/*
 * Pot.Web.Client
 * https://github.com/robert.freire/Pot.Web.Client
 *
 * Copyright (c) 2014 robert.freire
 * Licensed under the MIT license.
 */

(function($) {

  // Collection method.
  $.fn.Pot_Web_Client = function() {
    return this.each(function(i) {
      // Do something awesome to each selected element.
      $(this).html('awesome' + i);
    });
  };

  // Static method.
  $.Pot_Web_Client = function(options) {
    // Override default options with passed-in options.
    options = $.extend({}, $.Pot_Web_Client.options, options);
    // Return something awesome.
    return 'awesome' + options.punctuation;
  };

  // Static method default options.
  $.Pot_Web_Client.options = {
    punctuation: '.'
  };

  // Custom selector.
  $.expr[':'].Pot_Web_Client = function(elem) {
    // Is this element awesome?
    return $(elem).text().indexOf('awesome') !== -1;
  };

}(jQuery));
