/* eslint-disable import/no-extraneous-dependencies */
const { createBundleRenderer } = require('vue-server-renderer');
const serverBundle = require('./dist-server/vue-ssr-server-bundle.json');
const clientManifest = require('./dist-client/vue-ssr-client-manifest.json');

module.exports = {
	prerender: (callback, request) => {
		const context = { url: request.path };
		const renderer = createBundleRenderer(serverBundle, {
			runInNewContext: false,
			clientManifest,
			inject: false
		});

		renderer.renderToString(context, (err, html) => {
			if (err) {
				callback(err, null);
			} else {
				const res = {
					styles: context.renderStyles(),
					state: context.renderState(),
					scripts: context.renderScripts(),
					content: html,
					resourceHints: context.renderResourceHints(),
					preloadFIles: context.getPreloadFiles(),
					isNotFound: context.componentName === 'NotFound'
				};

				callback(null, res);
			}
		});
	}
};
