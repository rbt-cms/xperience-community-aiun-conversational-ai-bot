import React, { useState } from "react";
import '../styles/aiun-module.css';
// Props received from backend
interface AIUNTokenUsageLayoutProperties {
    readonly overallUsage: OverallUsage;
    readonly clients: TokenUsageClient[];
}

interface OverallUsage {
    readonly tokenLimit: number;
    readonly tokenUsed: number;
}

interface TokenUsageClient {
    readonly clientId: number;
    readonly clientName: string;
    readonly tokensUsed: number;
}

export const TokensUsageLayoutTemplate = ({
    overallUsage,
    clients,
}: AIUNTokenUsageLayoutProperties) => {

  

    return (
        <div className="token-usage-wrapper">
            <div>
                <div className="size-m___exMBL token-usage-text">Token Usage Summary</div>
                <table className="aiun-table-wrapper">
                    <thead>
                        <tr>
                            <th>Tokens Limit</th>
                            <th>Tokens Used</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td className="w-35">
                                {overallUsage.tokenLimit.toLocaleString()}
                            </td>
                            <td className="w-35">
                                {overallUsage.tokenUsed.toLocaleString()}
                            </td>
                        </tr>

                    </tbody>
                </table>

            </div>


            {clients && clients.length > 0 ? (
                <div>
                    <div className="size-m___exMBL usage-summary-text">Channel Usage Summary</div>
                    <table className="aiun-table-wrapper">
                        <thead>
                            <tr>
                                <th>Channel</th>
                                <th>Tokens Used</th>
                            </tr>
                        </thead>
                        <tbody>
                            {clients.map((client, idx) => (
                                <tr>
                                    <td className="w-35">{client.clientName}</td>
                                    <td className="w-35">
                                        {client.tokensUsed.toLocaleString()}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>

                </div>) : ""}

        </div>
    );
};

