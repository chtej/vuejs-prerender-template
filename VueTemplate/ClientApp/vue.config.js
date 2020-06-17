const NodeExternals = require('webpack-node-externals');
const VueSSRServerPlugin = require('vue-server-renderer/server-plugin');
const VueSSRClientPlugin = require('vue-server-renderer/client-plugin');
const path = require('path');

const serverConfig = {
	entry: './src/entry-server.js',
	target: 'node',
	devtool: 'source-map',
	output: {
		libraryTarget: 'commonjs2'
	},
	externals: NodeExternals({
		whitelist: /\.css$/
	}),
	optimization: {
		splitChunks: false
	},
	// This is the plugin that turns the entire output of the server build
	// into a single JSON file. The default file name will be
	// `vue-ssr-server-bundle.json`
	plugins: [
		new VueSSRServerPlugin()
	]
};

const cilentConfig = {
	entry: './src/entry-client.js',
	optimization: {
		runtimeChunk: {
			name: 'manifest'
		},
		splitChunks: {
			cacheGroups: {
				vendor: {
					test: /[\\/]node_modules[\\/]/,
					name: 'vendor',
					chunks: 'all'
				}
			}
		}
	},
	plugins: [
		// This plugins generates `vue-ssr-client-manifest.json` in the
		// output directory.
		new VueSSRClientPlugin()
	]
};

module.exports = {
	outputDir: `dist-${process.env.TARGET_ENV}`,
	configureWebpack: process.env.TARGET_ENV === 'server' ? serverConfig : cilentConfig,
	chainWebpack: (config) => {
		// remove index.html
		if (process.env.TARGET_ENV === 'client') {
			config.plugins.delete('html');
			config.plugins.delete('preload');
			config.plugins.delete('prefetch');
		}
	},
	productionSourceMap: false,
	publicPath: process.env.PUBLIC_PATH || '/',
	devServer: {
		contentBase: path.join(__dirname, 'dist-client'),
		hot: true,
		port: 9999,
		sockHost: '127.0.0.1',
		transportMode: 'ws',
		historyApiFallback: true,
		headers: {
			'Access-Control-Allow-Origin': '*'
		}
	}
};
